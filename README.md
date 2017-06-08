# N-Matrix-Programmer
A program to create a program that calculates inverse and determinant of N by N matrix.

![Screenshot](Info/Screenshot.png)

## Background

This program is created for programmers who want to write the program which will do inverse and determinant of N by N matrix.

For those who need it, it creates the code for you automatically in an instant, therefore saves you hours (even days) of time.

The output syntaxes and N-order can be changed via code or command-line arguments.

## Output samples
Here's one of output in valid C# code: [1x1](Info/Matrix_1x1.txt) [2x2](Info/Matrix_2x2.txt) [3x3](Info/Matrix_3x3.txt) [4x4](Info/Matrix_4x4.txt) [5x5](Info/Matrix_5x5.txt) [6x6](Info/Matrix_6x6.txt) [7x7](Info/Matrix_7x7.txt) [8x8](Info/Matrix_8x8.txt) [9x9](Info/Matrix_9x9.txt) [10x10](Info/Matrix_10x10.txt).

## Warning
The computation time (including output code size and processing memory) is [O(N!N^3)](http://www.cg.info.hiroshima-cu.ac.jp/~miyazaki/knowledge/teche23.html) as its complexity always increased over N.

However, for N>=4 The output steps is cached in local variables progressively for every (N-1), therefore the computation time is only O(N!), making the most efficient code that you'll ever see.

## License
The program and its generated code are both licensed as [MIT](LICENSE)