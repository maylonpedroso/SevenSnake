# SevenSnake v0.3.2

Problem solution:

Generate all snakes starting in each cell and keep a record for every obtained sum. Snakes duplication avoided during generation. The input csv file is read on demand until solution is found. Keeps in memory only the needed rows to calculate the snakes starting in current row.

* Usage: 
    
    SevenSnakesSearch.exe path/to/file.csv

* Output format: 

    [(row, col),(row, col),(row, col),(row, col),...]
    
    [(row, col),(row, col),(row, col),(row, col),...]

* Detailed analysis: 

    https://docs.google.com/document/d/1UM6-RqlijnUwNm-_Jv5a2h5AYPBwnRW2rWhToMgCOS4/edit#heading=h.wnng00d2vkc3	

Random grid generator:

* Usage: 

   GridGenerator.exe size path/to/file.csv 
