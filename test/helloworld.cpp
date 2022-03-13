#include <iostream>
#include <vector>


#include <algorithm>

using namespace std;

class Solution {
public:
<<<<<<< HEAD
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
=======
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
>>>>>>> 4013dfdc6e10695a93470c050dd463a84fc7dcd7
        }
    }
};