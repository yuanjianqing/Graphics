//
// Created by LEI XU on 4/27/19.
//

#ifndef RASTERIZER_TEXTURE_H
#define RASTERIZER_TEXTURE_H
#include "global.hpp"
#include <eigen3/Eigen/Eigen>
#include <opencv2/opencv.hpp>



class Texture{
private:
    cv::Mat image_data;

public:
    Texture(const std::string& name)
    {
        image_data = cv::imread(name);
        cv::cvtColor(image_data, image_data, cv::COLOR_RGB2BGR);
        width = image_data.cols;
        height = image_data.rows;
    }

    int width, height;

    Eigen::Vector3f getColor(float u, float v)
    {
        if (u < 0) u = 0;
		if (u > 1) u = 1;
		if (v < 0) v = 0;
		if (v > 1) v = 1;
        auto u_img = u * width;
        auto v_img = (1 - v) * height;
        auto color = image_data.at<cv::Vec3b>(v_img, u_img);
        return Eigen::Vector3f(color[0], color[1], color[2]);
    }

    cv::Vec3b lerp( float f, cv::Vec3b x, cv::Vec3b y)
    {
        cv::Vec3b result;
        result[0] = (y[0] - x[0]) * f + x[0];
        result[1] = (y[1] - x[1]) * f + x[1];
        result[2] = (y[2] - x[2]) * f + x[2];
        return result;
    }

    //双线性纹理插值
    Eigen::Vector3f getColorBilinear(float u, float v)
    {
        /*if (u < 0) u = 0;
		if (u > 1) u = 1;
		if (v < 0) v = 0;
		if (v > 1) v = 1;
        auto u_img = u * width;
        auto v_img = (1.0 - v) * height;

        //u_img -= 0.5;
        //v_img -= 0.5;
        float x, y;

        //下面的代码是一堆纹
        //float v00[2] = {MIN(std::floor(u_img), 0), MIN(std::floor(v_img), 0)};
        //float v10[2] = {MAX(std::ceil(u_img), (float)width), MIN(std::floor(v_img), 0)};
        //float v01[2] = {MIN(std::floor(u_img), 0), MAX(std::ceil(v_img), (float)height)};
        //float v11[2] = {MAX(std::ceil(u_img), (float)width), MAX(std::ceil(v_img), (float)height)};
        //
        float v00[2] = {MAX(std::floor(u_img), 0), MAX(std::floor(v_img), 0)};
        float v10[2] = {MIN(std::ceil(u_img), (float)width), MAX(std::floor(v_img), 0)};
        float v01[2] = {MAX(std::floor(u_img), 0), MIN(std::ceil(v_img), (float)height)};
        float v11[2] = {MIN(std::ceil(u_img), (float)width), MIN(std::ceil(v_img), (float)height)};

        float w = 1 - u_img;
        float h = 1 - v_img;
     
        auto color00 = image_data.at<cv::Vec3b>(v00[0], v00[1]);
        auto color10 = image_data.at<cv::Vec3b>(v10[0], v10[1]);
        auto color01 = image_data.at<cv::Vec3b>(v01[0], v01[1]);
        auto color11 = image_data.at<cv::Vec3b>(v11[0], v11[1]);

        auto colorlx1 = lerp(w,color00, color10 );
        auto colorlx2 = lerp( w,color01, color11);
        auto colorly = lerp( h,colorlx1, colorlx2);
        
        return Eigen::Vector3f(colorly[0], colorly[1], colorly[2]); 
        */
        if (u < 0) u = 0;
		if (u > 1) u = 1;
		if (v < 0) v = 0;
		if (v > 1) v = 1;
        auto u_img = u * width;
        auto v_img = (1 - v) * height;
        float x = u_img - std::floor(u_img);
        float y = v_img - std::floor(v_img);
        int x_flag = x < 0.5f ? -1 : 1;
        int y_flag = y < 0.5f ? -1 : 1;
        cv::Point2f p00 = cv::Point2f(std::floor(u_img) + 0.5f, std::floor(v_img) + 0.5f);
        cv::Point2f p01 = cv::Point2f(std::floor(u_img) + x_flag + 0.5f, std::floor(v_img) + 0.5f);
        cv::Point2f p10 = cv::Point2f(std::floor(u_img) + 0.5f, std::floor(v_img) + y_flag + 0.5f);
        cv::Point2f p11 = cv::Point2f(std::floor(u_img) + x_flag + 0.5f, std::floor(v_img) + y_flag + 0.5f);
        
        std::vector<cv::Point2f> vec;
        vec.push_back(p01);
        vec.push_back(p10);
        vec.push_back(p11);

        cv::Point2f dis = cv::Point2f(p00.x - u_img, p00.y - v_img);
        float len = sqrt(dis.x * dis.x + dis.y * dis.y);

        auto color00 = image_data.at<cv::Vec3f>(p00.x, p00.y);
        auto color01 = image_data.at<cv::Vec3f>(p01.x, p01.y);
        auto color10 = image_data.at<cv::Vec3f>(p10.x, p10.y);
        auto color11 = image_data.at<cv::Vec3f>(p11.x, p11.y);

        auto colorx1 = lerp(1, color00, color10);
        auto colorx2 = lerp(1, color01, color11);
        auto colory = lerp(1, colorx1, colorx2);
        
        
        return Eigen::Vector3f(colory[0], colory[1], colory[2]);
       
    }

};
#endif //RASTERIZER_TEXTURE_H
