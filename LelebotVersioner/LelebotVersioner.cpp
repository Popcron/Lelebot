#include <iostream>
#include <string>
#include <fstream>
#include <sstream>

using namespace std;

int main()
{
    //read whole string in
    string targetLine = "public const uint Version = ";
    ifstream recordingFile("Info.cs");
    std::stringstream data;
    data << recordingFile.rdbuf();
    string dataString = data.str();

    //find the end of the line
    int versionConstIndex = dataString.find(targetLine) + targetLine.length();
    int semicolonIndex = versionConstIndex;
    while (true)
    {
        if (dataString[semicolonIndex] == ';')
        {
            break;
        }

        semicolonIndex++;
    }

    //get version number and increment it
    string versionString = dataString.substr(versionConstIndex, semicolonIndex - versionConstIndex);
    int version = stoi(versionString);
    version++;

    //write to the string
    dataString.erase(versionConstIndex, versionString.length());
    dataString.insert(versionConstIndex, to_string(version));

    //clear the file
    ofstream ofs;
    ofs.open("Info.cs", ofstream::out | ofstream::trunc);
    ofs.close();

    //write the whole string back in
    fstream logFile;
    logFile.open("Info.cs", fstream::app);
    logFile << dataString;
    logFile.close();

    //print the new version
    printf(to_string(version).c_str());
}