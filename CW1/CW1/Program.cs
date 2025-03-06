
static double GetAvarage(int[] arrChangedNameYetAgain) 
{
    return (double) arrChangedNameYetAgain.Sum()/arrChangedNameYetAgain.Length;
}

static int GetMaxValue(int[] arr)
{
    return arr.Max();
} 

int[] arr = { 1, 2, 3, 4, 5 ,5};
Console.WriteLine(GetAvarage(arr));
Console.WriteLine(GetMaxValue(arr));