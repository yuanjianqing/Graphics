#include <iostream>
#include <vector>
using namespace std;
class Solution {
public:
    int search(vector<int>& nums, int target) 
    {
        int capacity = nums.size();
        int leftIndex = 0;
        int rightIndex = capacity - 1;
        int middleIndex = (leftIndex + rightIndex) / 2;
        while(nums[middleIndex] != target)
        {
            if(middleIndex <= 0 || middleIndex >= capacity - 1)
            {
                return -1;
            }
            if(nums[middleIndex] > target)
            {
                //如果中心大于 “目标” ，说明 “目标” 在 “中心” 左边， 左移 “右边” 和 “中心”
                rightIndex = middleIndex - 1;
                middleIndex = (leftIndex + rightIndex) / 2;
            }
            else
            {
                //如果中心大于 “目标” ， 说明 “目标” 在 “中心” 右边， 右移 “左边” 和 “中心”
                leftIndex = middleIndex + 1;
                middleIndex = (leftIndex + rightIndex) / 2;
            }
        }
        return middleIndex;
    }
};