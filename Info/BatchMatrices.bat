
@echo off
for /l %%x in (1, 1, 10) do (
	MatrixProgrammer.exe %%x > Matrix_%%xx%%x.txt
)