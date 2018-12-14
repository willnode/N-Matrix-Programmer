@echo off
for /l %%x in (1, 1, 10) do (
	MatrixProgrammer.exe -n %%x -f cs > Matrix_%%xx%%x_cs.txt
	MatrixProgrammer.exe -n %%x -f cpp > Matrix_%%xx%%x_cpp.txt
)