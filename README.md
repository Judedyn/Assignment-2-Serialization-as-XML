# Assignment 2 Shape Serialization  

## Description  
This console application shows how to save and load data using XML in C#.  

The program creates a list of shapes like Circle and Rectangle, saves them into a file, and then loads them back.  

Each shape has an Area property that is automatically calculated based on its size (radius, height, width). The area is not saved in the file, it is calculated again when the program runs.  

---

## How to Run  
1. Open the project folder in terminal  
2. Type:  
   dotnet build  
3. Then run:  
   dotnet run  

---

## What the Program Does  
- Creates a list of shapes  
- Saves the shapes to an XML file  
- Loads the shapes from the XML file  
- Displays the shapes and their areas  

---

## Example Output  
Loading shapes from XML:  
Circle is Red and has an area of 19.63  
Rectangle is Blue and has an area of 200  
Circle is Green and has an area of 201.06  
Circle is Purple and has an area of 475.29  
Rectangle is Blue and has an area of 810  

---

## How Area is Calculated  
- Circle → Area = π × radius × radius  
- Rectangle → Area = height × width  

---

## Error Handling  
The program handles errors so it will not crash:  

- Invalid values (like negative numbers)  
- Missing file  
- Invalid XML format  
- File read/write errors  

---

## Notes  
- Area is not saved in XML because it is calculated  
- The program checks data before saving and after loading  
- Uses classes and inheritance (Shape, Circle, Rectangle)  
