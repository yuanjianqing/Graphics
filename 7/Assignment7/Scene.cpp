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


    auto intersection = intersect(ray);
    if(!intersection.happened)
        return {};
    if(intersection.m->hasEmission())
        return intersection.m->getEmission();

    Vector3f L_dir = { 0,0,0 };
    Vector3f L_indir = { 0,0,0 };

    Intersection LightInter;
    float lightPdf;
    sampleLight(LightInter, lightPdf);

    Vector3f object2Light = LightInter.coords - intersection.coords;
    float disSq = object2Light.squrenorm();
    Vector3f light2ObjectDir = object2Light.normalized();
    Ray light2ObjectRay(intersection.coords, light2ObjectDir);

    Intersection light2Object_point = intersect(light2ObjectRay);

    //通过距离判断light是否打到物体上有没有遮挡

    if(light2Object_point.distance - object2Light.norm() > -EPSILON)
        L_dir = LightInter.emit * intersection.m->eval(ray.direction, object2Light, intersection.normal) * dotProduct(intersection.normal, object2Light) * dotProduct(-object2Light, LightInter.normal) / disSq / lightPdf;

    if(get_random_float() > RussianRoulette)
        return L_dir;
    
    Vector3f wo = intersection.m->sample(ray.direction, intersection.normal).normalized();
    Ray nextObjectRay(intersection.coords, wo);
    Intersection nextInter = intersect(nextObjectRay);

    if(nextInter.happened && !nextInter.m->hasEmission())
    {
        float pdf = intersection.m->pdf(ray.direction, wo, intersection.normal);
        L_indir = castRay(nextObjectRay, depth + 1) * intersection.m->eval(ray.direction, wo, intersection.normal) * dotProduct(wo, intersection.normal) / pdf / RussianRoulette;
    }

    return L_dir + L_indir;
}