#include <iostream>
#include <vector>


#include <algorithm>

using namespace std;
class Solution {
public:
    void rotate(vector<int>& nums, int k)
    {
        //1
        /*
        int n = nums.size();
        k = k % n;
        vector<int> ans(n);
        for(int i = 0; i < k; i++)
        {
            ans[i] = nums[n - k + i];
        }
        for(int i = k; i < n; i++)
        {
            ans[i] = nums[i - k];
        }
        swap(nums, ans);
        */
        
        //2
        
        int n = nums.size();
        k = k % n;
        for(int i = 0; i < k; i++)
        {
            int t = nums[n - 1];
            for(int j = n - 1; j > 0; j--)
            {
                nums[j] = nums[j - 1];
            }
            nums[0] = t;
        }
    }
};