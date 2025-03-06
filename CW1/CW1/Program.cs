
static double GetAvarage(int[] arrChangedName) 
{
    return (double) arrChangedName.Sum()/arrChangedName.Length;
}

static int GetMaxValue(int[] arr)
{
    return arr.Max();
} 

int[] arr = { 1, 2, 3, 4, 5 ,5};
Console.WriteLine(GetAvarage(arr));
Console.WriteLine(GetMaxValue(arr));