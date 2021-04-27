#include <iostream>
#include "tester.h"
#include <boost/asio.hpp> 



int main(int testNumber, char** ipAndPort)
{
  //call all tests here
  tester::testServerConnect();
  return 0; //number of tests failed
}

boost::asio::ip::tcp::socket tester::connectToServer(std::string serverip)
{
  //break serverip apart into port and address
  std::string delimiter = ":";
  std::string address = serverip.substr(0, serverip.find(delimiter));
  std::string port = serverip.substr(serverip.find(delimiter), serverip.length());
  
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

int tester::testServerConnect()
{
  //connect with socket to sever and start handshake
  std::string address;
  address = "127.0.0.1:1100";
  boost::asio::ip::tcp::socket socket = connectToServer(address);
  //send a string to the server as a username and expect a response
  boost::system::error_code error;
  boost::asio::write( socket, boost::asio::buffer("username"), error);
  if(!error)
    {
      return 0; // client has succesfully connected and sent a message
    }
  return 1; //return 1 if error
}

int tester::testCircularDependency()
{
  //connect to server and send a circular dependancy
  //ensure the server sends back the correct error response 


  return 0; 
}


int tester::testSimultaniousEdit()
{
  //send an edit of the same cell from two sockets to attempt for exception

  
  return 0;
}





