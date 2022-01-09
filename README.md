# XmlReader
This will read the data from xml file and save into the csv file


Step:1) Please clone/download the project from this repository. 

Step:2) I have used Visual Studio 2019 community edition with .net 5.

Step:3) Added an implementation for reading the xml file either from a specific user defined folder or from inside the project "XmlReader\XmlReader.FileWatcher\XmlFilies\'' location and copied the test xml file for this assignment purpose. So if incase you would like to provide input xml file from user defined folder location please open the "appsettings.json" file from (XmlReader.FileWatcher\appsettings.json) project location and update the "ReadFromLocalFolder" value to "true" and please update the "XmlFilePath" value.

This process will parse the "test.xml" file and perform following validations and save the data into two CSV files in either of the folder locations (step 3). 

Process I have followed to implement this solution:

    -> Read the xml file either from user defined folder location or from inside the project folder.

    -> Serialized xml file to aseXML C# class

    -> Validated to Header,Transactions,CsvIntervalData elements to be present to xml file object

    -> Extracted CSVBlockElement data and splitted the rows data using commaI(,) separator using string extensions

Performed following validation for CSVBlockElement and save to CSV files.

        Created separate CSV file for each block of data within the CSVIntervalData element that starts with "200"

        Each CSV file has the "100" row as a header, and the "900" row as the trailer

        Each CSV file named from the second field in the "200" row

        Rows within the CSVIntervalData element can only start with "100", "200", "300","900"

        The CSVIntervalData element contains at least 1 row for each of "100", "200", "300","900"

        "100", "900" rows appear once inside the CSVIntervalData element

        "200" and "300" can repeat and will be within the header and trailer rows 

        "200" row is followed by at least 1 "300" row

        Removed leading and trailing white spaces, tabs, and additional newlines.
