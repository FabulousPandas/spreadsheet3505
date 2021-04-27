/*
 * The main class for dealing with the server code
 */ 
#include "server.h"
#include <iostream>
#include <iterator>
#include <algorithm>
#include <string>
#include "listener.h"

using namespace boost::asio;


int main()
{

    try
    {

	server the_server;

        io_context io_context;
        listener listen(io_context, the_server);
        io_context.run();

    }
    catch (std::exception& e)
    {
        std::cerr << e.what() << std::endl;
    }


    return 0;
}

std::string server::get_list_of_spreadsheets()
{
	boost::filesystem::path sheet_dir(directory); // Creates a path object at the folder specified to hold the spreadsheets
	std::string sheet_list; // String list of the spreadsheets to be returned for sending to the client

	boost::filesystem::recursive_directory_iterator end;
	std::vector<std::string> spreadsheet_list;
	// Iterates through every file in the directory the spreadsheets are saved to
	for (boost::filesystem::recursive_directory_iterator i(sheet_dir); i != end; i++)
	{
		boost::filesystem::path sheet = (*i); // Sets the sheet path to whichever spreadsheet the iterator is currently pointing at
		spreadsheet_list.push_back(sheet.string().substr(directory.length() + 1, sheet.string().length() - directory.length() - 1)); // Removes the filepath up until the actual spreadsheet file name and pushes the filename to the vector
	}

	if (spreadsheet_list.size() == 0)
		return "\n\n";

	std::sort(spreadsheet_list.begin(), spreadsheet_list.end(), alphabetical_compare); // Sorts the vector storing the spreadsheet filenames into alphabetical order

	for (int i = 0; i < spreadsheet_list.size(); i++)
	{
		sheet_list += spreadsheet_list.at(i) + '\n'; // Appends each spreadsheet file name to the string with an added newline between each filename
	}
	return sheet_list + '\n'; // Returns the whole list of files with an additional newline appended so the client knows it's the end of the list
}

int server::get_ID()
{
	int clientID = curID;
	curID++;
	return clientID;
}

/*
 * Compares 2 strings in complete alphabetical order (case independent)
 */
bool server::alphabetical_compare(std::string a, std::string b)
{
	std::transform(a.begin(), a.end(), a.begin(), [](unsigned char c){ return std::tolower(c); }); // Converts a to all lowercase	
	std::transform(b.begin(), b.end(), b.begin(), [](unsigned char c){ return std::tolower(c); }); // Converts b to all lowercase
	return a < b;
}

    void server::polling()
    {
        bool idle = false;
        bool loop = true;


        while (loop)
        {
            for (int i = 0; i< clients.size(); i++)
            {

            }
        }
    }








