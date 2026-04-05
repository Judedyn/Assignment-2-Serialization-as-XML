using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ShapeSerializerApp;

namespace ShapeSerializerApp
{
    [XmlInclude(typeof(Circle))]
    [XmlInclude(typeof(Rectangle))]
    public abstract class Shape
    {
        public string Colour { get; set; } = string.Empty;

        // Area is read-Only.
        // It is not stored in the XML; It is calculated form other properties.
        [XmlIgnore]
        public abstract double Area { get; }

        // Optional validation hook for derived classes.
        public abstract void Validate();
    }
    public class Circle : Shape
    {
        public double Radius { get; set; }

        public override double Area => Math.PI * Radius * Radius;

        public override void Validate()
        {
            if (Radius <= 0)
                throw new InvalidOperationException("Circle radius must be positive.");
        }
    }
}

public class Rectangle : Shape
{
    public double Height { get; set; }
    public double Width { get; set; }

    [XmlIgnore]
    public override double Area => Height * Width;

    public override void Validate()
    {
        if (Height <= 0 || Width <= 0)
        {
            throw new InvalidOperationException("Rectangle height cannot be negative ");
        }

        if (Width <= 0)
        {
            throw new InvalidOperationException("Rectangle width cannot be negative ");
        }
    }

}

internal class Program
{
    static void Main()
    {
        // This program demonstrates XML serilaization and deserialization of a polymorphic List.
        // The Area property is read only and is recalutated after deserialization.
        // Area is excluded form XML using [XmlIgnore], beacuse it depends on Radius/Height/Width.
        // Basic error handling is include for missing files, invalid shape data,
        // and general I/O Problems. 

        string fileXml = Path.Combine(AppContext.BaseDirectory, "shapes.xml");

        var listOfShapes = new List<Shape>
        {
            new Circle { Colour = "Red", Radius = 2.5 },
            new Rectangle { Colour = "Blue", Height = 20.0, Width = 10.0 },
            new Circle { Colour = "Green", Radius = 8 },
            new Circle { Colour = "Purple", Radius = 12.3 },
            new Rectangle { Colour = "Blue", Height = 45.0, Width = 18.0 }
        };

        var serializer = new XmlSerializer(typeof(List<Shape>));

        try
        {
            // Validate shapes before saving.
            foreach (Shape shape in listOfShapes)
            {
                shape.Validate();
            }

            // Create or OverWrite the XML file.
            using(FileStream stream = File.Create(fileXml))
            {
                serializer.Serialize(stream, listOfShapes);
            }

            // Load the XML file back into memory.
            List<Shape>? loadedShapesXml;
            using (FileStream stream = File.OpenRead(fileXml))
            {
                loadedShapesXml = serializer.Deserialize(stream) as List<Shape>;
            }

            if (loadedShapesXml is null)
            {
                Console.WriteLine("Deserialization failed: the XML did not produce a valid list of shapes.");
                return;
            }

            // Validate loaded data after deserialization too.
            foreach (Shape shape in loadedShapesXml)
            {
                shape.Validate();
            }
            Console.WriteLine("Loading shapes form XML:");
            foreach (Shape item in loadedShapesXml)
            {
                Console.WriteLine($"{item.GetType().Name} is {item.Colour} and has an area of {item.Area}");
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Error: the output directory was not found.");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Error: the XML file could not be found.");
        }
        catch (InvalidOperationException ex)
        {
            // XmlSerializer ofthen throws InvalidOperationException for malformed XML
            // Or when XML structure does not match the traget type.
            Console.WriteLine("Error: the XML data is Invalid or does not match the expected format.");
            Console.WriteLine($"Details: {ex.Message}");
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine("Error: one or more shape values are invalid.");
            Console.WriteLine($"Details: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Error: a file input/output problem accured.");
            Console.WriteLine($"Details: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error:");
            Console.WriteLine(ex.Message);
        }

    }
}