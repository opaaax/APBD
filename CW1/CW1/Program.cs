
static double GetAvarage(int[] arr) 
{
    return (double) arr.Sum()/arr.Length;
}

int[] arr = { 1, 2, 3, 4, 5 ,5};
Console.WriteLine(GetAvarage(arr));