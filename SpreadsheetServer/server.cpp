/*
 * The main class for dealing with the server code
 */ 
#include "server.h"
#include <iostream>
#include <iterator>
#include <string>
#include "listener.h"
#include <unistd.h>
#include <csignal>

using namespace boost::asio;

server* the_server;

/*
 * Called when the server shuts down due to
 * the proccess being terminated on the command line (ctrl + c)
 */
void signal_handler(int signum)
{
	// For every spreadsheet
	for (std::map<std::string, spreadsheet*>::iterator it = the_server->spreadsheets.begin(); it != the_server->spreadsheets.end(); it++)
	{
		spreadsheet* sheet = it->second;
		sheet->server_shutdown("Shutdown manually by the server maintainer");
	}
	exit(signum); // shuts down program
}

int main()
{

    try
    {

	the_server = new server;
    	std::signal(SIGINT, signal_handler); // Look for a server termination
        io_context io_context;
        listener listen(io_context, the_server);
	boost::thread t(boost::bind(&boost::asio::io_context::run, &io_context)); // listens for connections on a seperate thread
	the_server->polling(); // start checking the spreadsheets for requests it needs to handle
	t.join();
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

	for (std::map<std::string, spreadsheet*>::iterator i = spreadsheets.begin(); i != spreadsheets.end(); i++)
	{
		sheet_list += i->first + '\n'; // Appends each spreadsheet file name to the string with an added newline between each filename
	}
	return sheet_list + '\n'; // Returns the whole list of files with an additional newline appended so the client knows it's the end of the list
}

/*
 * Gets a spreadsheet object mapped to the specified filename
 */
spreadsheet* server::open_sheet(std::string filename)
{
	if (spreadsheets.find(filename) == spreadsheets.end()) // if the filename is not in the spreadsheet list
	{
		boost::filesystem::path sheet_dir(directory); // Creates a path object at the folder specified to hold the spreadsheets
		boost::filesystem::ofstream new_file(sheet_dir / filename);
		new_file.close();
		spreadsheet* new_sheet = new spreadsheet(directory + '/' + filename);
		spreadsheets.insert(std::pair<std::string, spreadsheet*>(filename, new_sheet));
	}

	return spreadsheets[filename];
}

/*
 * Add all of the saved spreadsheets into a (filename -> spreadsheet object) map
 */
void server::put_spreadsheets_in_map()
{

	boost::filesystem::path sheet_dir(directory); // Creates a path object at the folder specified to hold the spreadsheets

	boost::filesystem::recursive_directory_iterator end;
	// Iterates through every file in the directory the spreadsheets are saved to
	for (boost::filesystem::recursive_directory_iterator i(sheet_dir); i != end; i++)
	{
		boost::filesystem::path sheet = (*i); // Sets the sheet path to whichever spreadsheet the iterator is currently pointing at
		std::string filename = sheet.string().substr(directory.length() + 1, sheet.string().length() - directory.length() - 1); // Removes the filepath up until the actual spreadsheet file name
		spreadsheet* new_sheet = new spreadsheet(directory + '/' + filename);
		spreadsheets.insert(std::pair<std::string, spreadsheet*>(filename, new_sheet));
	}

}

/*
 * Gets the next client ID
 */
int server::get_ID()
{
	int clientID = curID;
	curID++;
	return clientID;
}

    /*
     * Has each spreadsheet handle the next request in its queue
     */
    void server::polling()
    {
        bool loop = true;
        while (loop)
        {
	    bool idle = true;
	    std::map<std::string, spreadsheet*>::iterator it;
            for (it = spreadsheets.begin(); it != spreadsheets.end(); it++)
            {
		    spreadsheet* cur_sheet = it->second;
		    if (cur_sheet->needs_to_proccess_message())
		    {
			    process_message(cur_sheet);
			    idle = false;
		    }
            }
	    if (idle)
	    	usleep(1 * 1000000 / 2);
        }
    }

void server::process_message(spreadsheet* cur_sheet)
{
	cur_sheet->proccess_next_message();
}







