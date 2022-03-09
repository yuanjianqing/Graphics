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

    cv::Vec3b lerp(cv::Vec3b x, cv::Vec3b y, float f)
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
        if (u < 0) u = 0;
		if (u > 1) u = 1;
		if (v < 0) v = 0;
		if (v > 1) v = 1;
        auto u_img = u * width;
        auto v_img = (1 - v) * height;

        u_img -= 0.5;
        v_img -= 0.5;
        Eigen::Vector2f v00 = {std::floor(u_img) + 0.5, std::floor(v_img)+ 0.5};
        Eigen::Vector2f v10 = {std::ceil(u_img)+ 0.5, std::floor(v_img)+ 0.5};
        Eigen::Vector2f v01 = {std::floor(u_img)+ 0.5, std::ceil(v_img)+ 0.5};
        Eigen::Vector2f v11 = {std::ceil(u_img)+ 0.5, std::ceil(v_img)+ 0.5};

        float w = v10.x() - (u_img + 0.5);
        float h = v11.y() - (v_img + 0.5);

        Eigen::Vector3f colorx1, colorx2, colory;
        
        auto color00 = image_data.at<cv::Vec3b>(v00.x(), v00.y());
        auto color10 = image_data.at<cv::Vec3b>(v10.x(), v10.y());
        auto color01 = image_data.at<cv::Vec3b>(v01.x(), v01.y());
        auto color11 = image_data.at<cv::Vec3b>(v11.x(), v11.y());

        auto colorlx1 = lerp(color00, color10, w);
        auto colorlx2 = lerp(color01, color11, w);
        auto colorly = lerp(colorlx1, colorlx2, h);
        
        return Eigen::Vector3f(colorly[0], colorly[1], colorly[2]); 
    }

};
#endif //RASTERIZER_TEXTURE_H
