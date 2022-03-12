#include <iostream>
#include <vector>
using namespace std;

class Solution {
public:
    void moveZeroes(vector<int>& nums) 
    {
        vector<int> positon;
        int n = nums.size();
        int p1 = 0, p2 = 0, count = 0, fake = 0;
        while(p1 <= n - 1 - count + positon.size())
        {
            if(nums[p1] == 0 || nums[p1] == -1)
            {
                count++;
                while(nums[p2] == 0 && p2 <= n - 1)
                {
                    p2++;
                }
                nums[p1] = nums[p2];
                nums[p2] = 0;
                fake++;
                p2++;
            }
            p1++;
        }
    }
};