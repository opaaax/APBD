﻿
static double GetAvarage(int[] arr) 
{
    return (double) arr.Sum()/arr.Length;
}

static int GetMax(int[] arr)
{
    return arr.Max();
}

int[] arr = { 1, 2, 3, 4, 5 ,5};
Console.WriteLine(GetAvarage(arr));
Console.WriteLine(GetMax(arr));