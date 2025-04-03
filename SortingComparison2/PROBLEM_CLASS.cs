namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class PROBLEM_CLASS
    {
        #region YOUR CODE IS HERE 

        //Your Code is Here:
        //==================
        /// <summary>
        /// Quick Insertion Sort: to speedup quicksort is to simply not sort arrays of size less than or equal certain threshold, 
        /// then use insertion sort on the entire array
        /// </summary>
        /// <param name="numbers">Array to be sorted</param>
        /// <param name="N">The array size</param>
        /// <param name="threshold">The quick insertion threshold at which to stop the quicksort</param>
        /// <returns>The sorted array</returns>
        static public float[] RequiredFunction(float[] numbers, int N, int threshold)
        {
            if (N <= 1)
            {
                return numbers;
            }

            QuickSort(numbers, 0, N - 1, threshold);
            return numbers;
        }

        private static void QuickSort(float[] nums, int l, int r, int threshold)
        {
            if (l < r)
            {
                if (r - l + 1 <= threshold)
                {
                    InsertionSortAlgorithm(nums, l, r);
                }
                else
                {
                    int pivot = Partition(nums, l, r);
                    QuickSort(nums, l, pivot - 1, threshold);
                    QuickSort(nums, pivot, r, threshold);
                }
            }
        }



        private static int Partition(float[] nums, int l, int r)
        {

            int mid = l + (r - l) / 2;

            float pivot = nums[mid];

            int i = l, j = r;


            while (i <= j)
            {
                while (nums[i] < pivot)
                {
                    i++;
                }
                while (nums[j] > pivot)
                {
                    j--;
                }
                if (i <= j)
                {
                    Swap(nums, i, j);
                    i++;
                    j--;
                }
            }
            return i;

        }



        private static void InsertionSortAlgorithm(float[] nums, int l, int r)
        {
            for (int i = l + 1; i <= r; i++)
            {
                float k = nums[i];
                int j = i - 1;
                while (j >= l && nums[j] > k)
                {
                    nums[j + 1] = nums[j];
                    j--;
                }
                nums[j + 1] = k;
            }
        }

        private static void Swap(float[] nums, int i, int j)
        {
            float temp = nums[i];
            nums[i] = nums[j];
            nums[j] = temp;
        }
        #endregion
    }
}
