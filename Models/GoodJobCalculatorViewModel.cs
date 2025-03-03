using System.Collections.Generic;

namespace Convenience.org.Models;

public class GoodJobCalculatorViewModel
{
    public string LogoImageUrl { get; set; }

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Step 1 - Revenue Fields
    public string Field0R { get; set; }
    public string Field1R { get; set; }
    public string Field3R { get; set; }
    public string Field4R { get; set; }
    public string Field5R { get; set; }
    public string Field6R { get; set; }

    // Step 2 - Cost Mitigation Fields
    public string Field0C { get; set; }
    public string Field7C { get; set; }
    public string Field2C { get; set; }
    public string Field3C { get; set; }
    public string Field4C { get; set; }
    public string Field5C { get; set; }
    public string Field6C { get; set; }
    public string Field11C { get; set; }
    public string Field8C { get; set; }
    public string Field9C { get; set; }
    public string Field10C { get; set; }

    // Step 3 - Calculated results

    // hidden fields
    public string Text0 { get; set; }
    public string Text1 { get; set; }
    public string Text2 { get; set; }
    public string Text3 { get; set; }
    public string Text4 { get; set; }
    public string Text5 { get; set; }
    public string Text6 { get; set; }
    public string Text7 { get; set; }
    public string Text8 { get; set; }
    public string Text9 { get; set; }
    public string Text10 { get; set; }
    public string Text11 { get; set; }
    public string Text12 { get; set; }
    public string Text13 { get; set; }
    public string Text14 { get; set; }
    public string Text15 { get; set; }
    public string Text16 { get; set; }
    public string Text17 { get; set; }
    public string Text18 { get; set; }


    public string CaField111 { get; set; }
    public string CaField2 { get; set; }
    public double Slider1 { get; set; } = 4.9;
    public string CaField33 { get; set; }
    public string CaField4 { get; set; }
    public double Slider2 { get; set; } = 1.7;
    public double CaField6 { get; set; } = 10;
    public string CaField8 { get; set; }
    public double Slider4 { get; set; } = -31.8;
    public double CaField999 { get; set; }
    public string CaField10 { get; set; }
    public double Slider5 { get; set; } = -28.6;
    public double CaField1111 { get; set; }
    public string CaField12 { get; set; }
    public double Slider6 { get; set; } = -13.7;
    public double CaField14 { get; set; } = 75;
    public double CaField15 { get; set; }
    public double CaField16 { get; set; }
    public double CaField17 { get; set; }
    public string ReField0 { get; set; }//
    public double ReField00 { get; set; }
    public string ReField1 { get; set; }//
    public double ReField11 { get; set; }
    public string ReField2 { get; set; }//
    public double ReField22 { get; set; }
    public string ReField3 { get; set; }//
    public double ReField33 { get; set; }
    public string ReField4 { get; set; }//
    public double ReField44 { get; set; }
    public string ReField5 { get; set; }//
    public double ReField55 { get; set; }

    public string ReField6 { get; set; }//
    public double ReField66 { get; set; }

    // label
    public string CaField1 { get; set; }
    public string CaField3 { get; set; }
    public string CaField7 { get; set; }
    public string CaField9 { get; set; }
    public string CaField11 { get; set; }

    public int CurrentStep { get; set; } = 1;
    public List<string> Steps { get; set; } = new List<string> { "Step 1: Revenue", "Step 2: Cost", "Step 3: Results", "Step 4: Take Action" };

    public bool Step1Completed { get; set; }
    public bool Step2Completed { get; set; }
    public bool Step3Completed { get; set; }
    public bool Step4Completed { get; set; }
    
}
