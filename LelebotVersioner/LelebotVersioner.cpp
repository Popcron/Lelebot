#include <iostream>
#include <string>
#include <fstream>
#include <sstream>

using namespace std;

void incrementVersion();
void deleteArtifacts();

int main(int argc, char* argv[])
{
    if (argc == 2)
    {
        string arg = string(argv[1]);
        if (arg == "--increment")
        {
            printf("incrementing version\n");
            incrementVersion();
        }
        else if (arg == "--clean")
        {
            printf("cleaing folder\n");
            deleteArtifacts();
        }
    }
}

void incrementVersion()
{
    //read whole string in
    ifstream versionFile("..\\version.txt");
    std::stringstream data;
    data << versionFile.rdbuf();
    string dataString = data.str();

    //get version number and increment it
    int version = stoi(dataString);
    version++;

    //write to the string
    dataString = to_string(version);

    //clear the file
    ofstream ofs;
    ofs.open("..\\version.txt", ofstream::out | ofstream::trunc);
    ofs.close();

    //write the whole string back in
    fstream logFile;
    logFile.open("..\\version.txt", fstream::app);
    logFile << dataString;
    logFile.close();
}

void deleteArtifacts()
{
    string command = "cd ..";
    system(command.c_str());

    command = "del *.xml /s /q";
    system(command.c_str());

    command = "del *.iobj /s /q";
    system(command.c_str());

    command = "del *.pdb /s /q";
    system(command.c_str());

    command = "del *.ipdb /s /q";
    system(command.c_str());

    command = "del Lelebot.exe.config /s /q";
    system(command.c_str());
}