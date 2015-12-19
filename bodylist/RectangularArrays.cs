//----------------------------------------------------------------------------------------
//	Copyright © 2007 - 2015 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class includes methods to convert Java rectangular arrays (jagged arrays
//	with inner arrays of the same length).
//----------------------------------------------------------------------------------------
namespace Com.CodeGame.CodeRacing2015.DevKit.CSharpCgdk
{
    public static class RectangularArrays
    {
        internal static Body[][][] ReturnRectangularBodyArray(int size1, int size2, int size3)
        {
            Body[][][] newArray = new Body[size1][][];
            for (int array1 = 0; array1 < size1; array1++)
            {
                newArray[array1] = new Body[size2][];
                if (size3 > -1)
                {
                    for (int array2 = 0; array2 < size2; array2++)
                    {
                        newArray[array1][array2] = new Body[size3];
                    }
                }
            }

            return newArray;
        }
    }
}