/*
 * The main class for dealing with the server code
 */ 
#include "server.h"
#include <iostream>
#include <iterator>
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

server::server()
{
	put_spreadsheets_in_map();
}

std::string server::get_list_of_spreadsheets()
{
	if (spreadsheets.size() == 0)
		return "\n\n";

	std::string sheet_list; // String list of the spreadsheets to be returned for sending to the client

	for (std::map<std::string, spreadsheet>::iterator i = spreadsheets.begin(); i != spreadsheets.end(); i++)
	{
		sheet_list += i->first + '\n'; // Appends each spreadsheet file name to the string with an added newline between each filename
	}
	return sheet_list + '\n'; // Returns the whole list of files with an additional newline appended so the client knows it's the end of the list
}

spreadsheet server::open_sheet(std::string filename)
{
	if (spreadsheets.find(filename) == spreadsheets.end()) // if the filename is not in the spreadsheet list
	{
		boost::filesystem::path sheet_dir(directory); // Creates a path object at the folder specified to hold the spreadsheets
		boost::filesystem::ofstream new_file(sheet_dir / filename);
		new_file.close();
		spreadsheet new_sheet(filename);
		spreadsheets.insert(std::pair<std::string, spreadsheet>(filename, new_sheet));
		return new_sheet;
	}

	spreadsheet dummy;
	return dummy;

}

void server::put_spreadsheets_in_map()
{

	boost::filesystem::path sheet_dir(directory); // Creates a path object at the folder specified to hold the spreadsheets

	boost::filesystem::recursive_directory_iterator end;
	// Iterates through every file in the directory the spreadsheets are saved to
	for (boost::filesystem::recursive_directory_iterator i(sheet_dir); i != end; i++)
	{
		boost::filesystem::path sheet = (*i); // Sets the sheet path to whichever spreadsheet the iterator is currently pointing at
		std::string filename = sheet.string().substr(directory.length() + 1, sheet.string().length() - directory.length() - 1); // Removes the filepath up until the actual spreadsheet file name
		spreadsheet new_sheet(filename);
		spreadsheets.insert(std::pair<std::string, spreadsheet>(filename, new_sheet));
	}

}

int server::get_ID()
{
	int clientID = curID;
	curID++;
	return clientID;
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








