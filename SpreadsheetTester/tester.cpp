#include <iostream>
#include "tester.h"
#include <boost/asio.hpp> 



int main(int argc, char** args)
{
  if(argc == 1)
    {
      std::cout << "num of tests" << std::endl;
    }
  // use a switch to find and run the correct test based on input


  //call all tests here
  tester::testServerConnect(args[2]);
  return 0; //number of tests failed
}

boost::asio::ip::tcp::socket tester::connectToServer(std::string serverip)
{
  //break serverip apart into port and address
  std::string delimiter = ":";
  std::string address = serverip.substr(0, serverip.find(delimiter));
  std::string port = serverip.substr(serverip.find(delimiter) + 1, serverip.length());
  
  boost::asio::io_context io_context;
  //create a socket to communicate on
  boost::asio::ip::tcp::socket socket(io_context);
  //connect to server on socket
  boost::system::error_code err;
  socket.connect( boost::asio::ip::tcp::endpoint( boost::asio::ip::address::from_string(address), std::stoi(port)), err);
  if(err)
	{
	  std::cout << "fail" << std::endl;
	  exit;
	}
  return socket;
		  
}

boost::asio::ip::tcp::socket tester::completeHandshake(std::string serverip, std::string username)
{
  boost::asio::ip::tcp::socket socket = connectToServer(serverip);
    boost::system::error_code err;
    boost::asio::write(socket, boost::asio::buffer(username + "\n"), err); // send username 
    if(!err)
    {
      std::vector<char> tempRec = std::vector<char>(100);
	boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 100); 
        boost::asio::read(socket, receive, boost::asio::transfer_all(), err); // get list of sheets
        if(!err)
        {
            boost::asio::write(socket, boost::asio::buffer(username + "\n"), err); // make new sheet
            if(!err)
            {
                char* data;
                std::string temp(data);
                while(temp.find("\n") != std::string::npos) // loop until all updates for the sever are processed
                {
                boost::asio::read(socket, receive, boost::asio::transfer_all(), err);
                if(!err)
                {
                    data = boost::asio::buffer_cast<char*>(receive);
                    temp = std::string(data);
                }
                }
            }
        } else 
        {
             std::cout << "fail" << std::endl;
             exit;
        }
        
    } else 
    {
        std::cout << "fail" << std::endl;
        exit;
    }
    return socket;
}

int tester::testServerConnect(std::string address)
{
  //connect with socket to sever and start handshake
  boost::asio::ip::tcp::socket socket = connectToServer(address);
  //send a string to the server as a username and expect a response
  boost::system::error_code error;
  boost::asio::write( socket, boost::asio::buffer("username \n"), error);
  if(error)
    {
      std::cout << "fail" << std::endl;
      return 0; // client has succesfully connected and sent a message
    }
    std::cout << "pass" << std::endl;
  return 1; //return 1 if error
}

int tester::testCircularDependency(std::string address)
{
  //connect to server and send a circular dependancy
  boost::asio::ip::tcp::socket socket = completeHandshake(address, "testCircularDependency"); // send test name as username for unique file
  boost::system::error_code err;
  //send first edit
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"}"));
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\", \"contents\":\"3\"}"));
  //clear buffer
  boost::asio::streambuf receive;
  boost::asio::read(socket, receive, boost::asio::transfer_all(), err); 
  //send second edit
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A2\"}"));
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A2\", \"contents\":\"=A1 + A2\"}"));
  //check buffer
  boost::asio::read(socket, receive, boost::asio::transfer_all(), err);
  const char* data = boost::asio::buffer_cast<const char*>(receive.data());
  std::string temp(data);
  //ensure the server sends back the correct error response 
  if(temp.find("requestError"))
  {
      std::cout << "pass" << std::endl;
  } else 
  {
      std::cout << "fail" << std::endl;
  }
  

  return 0; 
}


int tester::testSimultaniousEdit(std::string adress)
{
  //send an edit of the same cell from two sockets to ensure second edit occurs
  boost::asio::ip::tcp::socket socket1 = completeHandshake(address, "testSimultaniousEdit");
  boost::asio::ip::tcp::socket socket2 = completeHandshake(address, "testSimultaniousEdit");
 
  
  return 0;
}



