//
// Created by Göksu Güvendiren on 2019-05-14.
//

#include "Scene.hpp"


void Scene::buildBVH() {
    printf(" - Generating BVH...\n\n");
    this->bvh = new BVHAccel(objects, 1, BVHAccel::SplitMethod::NAIVE);
}

Intersection Scene::intersect(const Ray &ray) const
{
    return this->bvh->Intersect(ray);
}

void Scene::sampleLight(Intersection &pos, float &pdf) const
{
    float emit_area_sum = 0;
    for (uint32_t k = 0; k < objects.size(); ++k) {
        if (objects[k]->hasEmit()){
            emit_area_sum += objects[k]->getArea();
        }
    }
    float p = get_random_float() * emit_area_sum;
    emit_area_sum = 0;
    for (uint32_t k = 0; k < objects.size(); ++k) {
        if (objects[k]->hasEmit()){
            emit_area_sum += objects[k]->getArea();
            if (p <= emit_area_sum){
                objects[k]->Sample(pos, pdf);
                break;
            }
        }
    }
}

bool Scene::trace(
        const Ray &ray,
        const std::vector<Object*> &objects,
        float &tNear, uint32_t &index, Object **hitObject)
{
    *hitObject = nullptr;
    for (uint32_t k = 0; k < objects.size(); ++k) {
        float tNearK = kInfinity;
        uint32_t indexK;
        Vector2f uvK;
        if (objects[k]->intersect(ray, tNearK, indexK) && tNearK < tNear) {
            *hitObject = objects[k];
            tNear = tNearK;
            index = indexK;
        }
    }


    return (*hitObject != nullptr);
}

// Implementation of Path Tracing 
Vector3f Scene::castRay(const Ray &ray, int depth) const
{
    // TO DO Implement Path Tracing Algorithm here


    Vector3f L_dir = { 0,0,0 };
     Vector3f L_indir = { 0,0,0 };
 
     Intersection intersection = Scene::intersect(ray);
     if (!intersection.happened)
     {
         return {};
     }
     //打到光源
     if (intersection.m->hasEmission())
     return intersection.m->getEmission();
 
     //打到物体后对光源均匀采样
     Intersection lightpos;
     float lightpdf = 0.0f;
     sampleLight(lightpos, lightpdf);//获得对光源的采样，包括光源的位置和采样的pdf
 
     Vector3f collisionlight = lightpos.coords - intersection.coords;
     float dis = collisionlight.squrenorm();
     Vector3f collisionlightdir = collisionlight.normalized();
     Ray objray(intersection.coords, collisionlightdir);
 
     Intersection ishaveobj = Scene::intersect(objray);
     //L_dir = L_i * f_r * cos_theta * cos_theta_x / |x - p | ^ 2 / pdf_light
     if (ishaveobj.distance - collisionlight.norm() > -EPSILON)//说明之间没有遮挡
     L_dir = lightpos.emit * intersection.m->eval(ray.direction, collisionlightdir, intersection.normal) * dotProduct(collisionlightdir, intersection.normal) * dotProduct(-collisionlightdir, lightpos.normal) / dis / lightpdf;
 	 //打到物体后对半圆随机采样使用RR算法
     if (get_random_float() > RussianRoulette)
     return L_dir;
 
     Vector3f w0 = intersection.m->sample(ray.direction, intersection.normal).normalized();
     Ray objrayobj(intersection.coords, w0);
     Intersection islight = Scene::intersect(objrayobj);
     // shade(q, wi) * f_r * cos_theta / pdf_hemi / P_RR
     if (islight.happened && !islight.m->hasEmission())
     {
         float pdf = intersection.m->pdf(ray.direction, w0, intersection.normal);
         L_indir = castRay(objrayobj, depth + 1) * intersection.m->eval(ray.direction, w0, intersection.normal) * dotProduct(w0, intersection.normal) / pdf / RussianRoulette;
     }
 
     return L_dir + L_indir;
}