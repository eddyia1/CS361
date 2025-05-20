#include <iostream>
#include <chrono>
#include <thread>
#include <fstream>
#include <string>
#include <vector>
#include <sstream>

using namespace std;

//Checks if request.txt exists every 100ms
bool file_exists(string requestpath)
{
    ifstream file(requestpath);
    return file.is_open();
}

//Performs the calculation of average of the data in request.txt
float average_data(string requestpath)
{
    ifstream request(requestpath);
    string data;
    vector<float> numbers;
    
    getline(request, data);
    stringstream ss(data);
    string current;
    float sum = 0;

    while (getline(ss, current, ','))
    {
        try {
            numbers.push_back(stof(current));
        }
        //Given there is a non-number encountered, the program will thrown an exception and cease execution
        catch (invalid_argument ia)
        {
            cerr << "Encountered invalid char: " << current << " in request.txt" << endl;
            return -1;
        }
    }

    for (int i = 0; i < numbers.size(); i++)
    {
        sum += numbers[i];
    }

    return sum / float(numbers.size());
}

//Writes the calculated average to response.txt
void write_average(string responsepath, float avg)
{  
    ofstream file(responsepath);
    file << avg;
    file.close();
}

int main()
{
    const char* requestpath = "C:\\Users\\ipepd\\Downloads\\Main_Program\\Main_Program\\Main_Program\\database\\request.txt";
    const char* responsepath = "C:\\Users\\ipepd\\Downloads\\Main_Program\\Main_Program\\Main_Program\\database\\response.txt";

    float avg;

    while (true)
    {
        //The number of milliseconds to sleep for can be changed to optimize for performance, if necessary
        this_thread::sleep_for(chrono::milliseconds(100));

        if (file_exists(requestpath))
        {
            cout << "Found request.txt" << endl;

            //Get the average from the data array written to request.txt
            avg = average_data(requestpath);
            cout << "Average for this data was " << avg << endl;

            //Change request.txt to response.txt to allow for main program to view change
            remove(requestpath);
            write_average(responsepath, round(avg));
        }

        cout << "request.txt not found" << endl;
    }
}
