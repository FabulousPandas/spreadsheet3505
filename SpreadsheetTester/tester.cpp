#include <iostream>
#include "tester.h"
#include <boost/asio.hpp> 
#include <cstdlib>


int main(int argc, char** args)
{
  if(argc == 1)
    {
      std::cout << 9 << std::endl;
      return 0;
    }
  // use a switch to find and run the correct test based on input
  int testnum = atoi(args[1]);
  switch(testnum)
  {
    //call all tests here
  case 1:
    //print info before
    std::cout << "3" << std::endl;
    std::cout << "TestServerConnect" << std::endl;
    tester::testServerConnect(args[2]);
    break;
  case 2:
    std::cout << "20" << std::endl;
    std::cout << "TestCircularDependency" << std::endl;
    tester::testCircularDependency(args[2]);
    break;
  case 3:
    std::cout << "15" << std::endl;
    std::cout << "TestSimultaniousEdit" << std::endl;
    tester::testSimultaniousEdit(args[2]);
    break;
  case 4:
    std::cout << "15" << std::endl;
    std::cout << "TestIfServerSendsIntBackAsString" << std::endl;
    tester::testIfServerSendsIntBackAsString(args[2]);
    break;
  case 5:
    std::cout << "15" << std::endl;
    std::cout << "TestOfServerSemdsStromgBackAsString" << std::endl;
    tester::testIfServerSendsStringBackAsString(args[2]);
    break;
  case 6:
    std::cout << "15" << std::endl;
    std::cout << "TestClientDisconnectIsInformed" << std::endl;
    tester::testClientDisconnectIsInformed(args[2]);
    break;
  case 7:
    std::cout << "15" << std::endl;
    std::cout << "TestUndo" << std::endl;
    tester::testUndo(args[2]);
    break;
  case 8:
    std::cout << "15" << std::endl;
    std::cout << "TestEditAndUndoDifferentClient" << std::endl;
    tester::testEditAndUndoDifferentClient(args[2]);
    break;
  case 9:
    std::cout << "30" << std::endl;
    std::cout << "TestSpamEdits" << std::endl;
    tester::testSpamEdits(args[2]);
  }
  return 0; 
}
/**
 * helper to connect to the server
 **/
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
	  std::cout << "fail \n" << std::endl;
	  exit(EXIT_FAILURE);
	}
  return socket;
		  
}
/**
 * completes the handshake as a helper method for testing
 **/
boost::asio::ip::tcp::socket tester::completeHandshake(std::string serverip, std::string username)
{
    boost::asio::ip::tcp::socket socket = connectToServer(serverip);
    boost::system::error_code err;
    boost::asio::write(socket, boost::asio::buffer(username + "\n"), err); // send username 
    if(!err)
    {
      std::vector<char> tempRec = std::vector<char>(1000);
      boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 1000);
      socket.read_some(receive, err); // get list of sheets
      if(!err)
        {
	  boost::asio::write(socket, boost::asio::buffer(username + "\n"), err); // make new sheet
            if(!err)
            {
	      usleep(3 * 1000000);   
            }
        } else 
        {
             std::cout << "fail \n" << std::endl;
             exit(EXIT_FAILURE);
        }
        
    } else 
    {
        std::cout << "fail \n" << std::endl;
        exit(EXIT_FAILURE);
    }
    return socket;
}
/**
 * tests if the tester can connect to a server correct
 **/
void tester::testServerConnect(std::string address)
{
  //connect with socket to sever and start handshake
  boost::asio::ip::tcp::socket socket = connectToServer(address);
  //send a string to the server as a username and expect a response
  boost::system::error_code error;
  boost::asio::write( socket, boost::asio::buffer("username \n"), error);
  if(error)
    {
      std::cout << "fail \n" << std::endl;
      
    } else 
    {
      std::cout << "pass \n" << std::endl;
    }
}
/**
 * testing if the server recognises a circular dependancy
 **/
void tester::testCircularDependency(std::string address)
{
  //connect to server and send a circular dependancy
  boost::asio::ip::tcp::socket socket = completeHandshake(address, "testCircularDependency"); // send test name as username for unique file
  boost::system::error_code err;
  //send first edit
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"} \n"));
  usleep(1* 1000000 /4);
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"3\"} \n"));
  usleep(1* 1000000 /4);
  //clear buffer
  std::vector<char> tempRec = std::vector<char>(500);
  boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 500); 
  socket.read_some( receive, err); 
  //send second edit
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A2\"} \n"));
  usleep(1* 1000000 /4);
  //clear buffer
  socket.read_some(receive, err);
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A2\", \"contents\":\"=A1 + A2\"} \n"));
  usleep(3 *1000000);
  //check buffer
  socket.read_some(receive, err);
  const char* data = boost::asio::buffer_cast<const char*>(receive);
  std::string temp(data);
  //ensure the server sends back the correct error response 
  std::cout << temp << std::endl;
  if(temp.find("requestError") != std::string::npos && !err)
  {
      std::cout << "pass \n" << std::endl;
  } else 
  {
      std::cout << "fail \n" << std::endl;
  }
}

/**
 * tests to see if two message sent back to back result in the second
 * message is the one saved 
 **/
void tester::testSimultaniousEdit(std::string address)
{
  //send an edit of the same cell from two sockets to ensure second edit occurs
  boost::asio::ip::tcp::socket socket1 = completeHandshake(address, "testSimultaniousEdit");
  boost::asio::ip::tcp::socket socket2 = completeHandshake(address, "testSimultaniousEdit");
  //prep sockets by selecting a cell
  boost::asio::write(socket1, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"} \n"));
  usleep(1* 1000000 /4);
  boost::asio::write(socket2, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"} \n"));
  usleep(1* 1000000 /4);
  //send different message across both sockets 
  boost::asio::write(socket1, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"3\"} \n"));
  usleep(1* 1000000 /4);
  boost::asio::write(socket2, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"4\"} \n"));
  usleep(1* 1000000 /4);
  //check to make sure that the second value is the one selected
  boost::system::error_code err;
  std::vector<char> tempRec = std::vector<char>(500);
  boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 500); 
  socket2.read_some(receive, err);
  const char* data = boost::asio::buffer_cast<const char*>(receive);
  std::string temp(data);
  if(temp.find("4") != std::string::npos && temp.find("cellUpdated") != std::string::npos &&  !err)
  {
    std::cout << "pass \n" << std::endl;
  } else 
  {
    std::cout << "fail \n" << std::endl;
  }
}
/**
 * tests if an int change gets sent back as a string
 **/
void tester::testIfServerSendsIntBackAsString(std::string address)
{
  //connect a socket
  boost::asio::ip::tcp::socket socket = completeHandshake(address, "testIfServerSendsIntBackAsString");
  //select a cell
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"} \n"));
  usleep(1* 1000000 /4);
  //make a change request
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"3\"} \n"));
  //confirm server sends it back
  usleep(1 * 1000000 /4);
  boost::system::error_code err;
  std::vector<char> tempRec = std::vector<char>(500);
  boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 500); 
  socket.read_some(receive, err);
  const char* data = boost::asio::buffer_cast<const char*>(receive);
  std::string temp(data);
  if(temp.find("cellUpdated") != std::string::npos && temp.find("3") != std::string::npos && !err)
  {
    std::cout << "pass \n" << std::endl;
  } else 
  {
    std::cout << "fail \n" << std::endl;
  }
}
/**
 * tests if a string change gets sent back as a string
 **/
void tester::testIfServerSendsStringBackAsString(std::string address)
{
  //connect a socket
  boost::asio::ip::tcp::socket socket = completeHandshake(address, "testIfServerSendsStringBackAsString");
  //select a cell
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"} \n"));
  usleep(1* 1000000 /4);
  //make a change request
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"test\"} \n"));
  usleep(1* 1000000 /4);
  //confirm server sends it back
  boost::system::error_code err;
  std::vector<char> tempRec = std::vector<char>(500);
  boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 500); 
  socket.read_some(receive, err);
  const char* data = boost::asio::buffer_cast<const char*>(receive);
  std::string temp(data);
  if(temp.find("cellUpdated") != std::string::npos && temp.find("test") != std::string::npos && !err)
  {
    std::cout << "pass \n" << std::endl;
  } else 
  {
    std::cout << "fail \n" << std::endl;
  }
}

void tester::testClientDisconnectIsInformed(std::string address)
{
  //connect two clients and disconnect one
  boost::asio::ip::tcp::socket socket1 = completeHandshake(address, "testClientDisconnectIsInfromed");
  boost::asio::ip::tcp::socket socket2 = completeHandshake(address, "testClientDisconnectIsInfromed");
  //diconect one client 
  socket1.close();
  usleep(1*1000000);
  boost::system::error_code err;
  std::vector<char> tempRec = std::vector<char>(500);
	boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 500); 
  socket2.read_some(receive, err);
  const char* data = boost::asio::buffer_cast<const char*>(receive);
  std::string temp(data);
  if(temp.find("disconnected") != std::string::npos && !err)
  {
    std::cout << "pass \n" << std::endl;
  } else 
  {
    std::cout << "fail \n" << std::endl;
  }
}

void tester::testUndo(std::string address)
{
  boost::asio::ip::tcp::socket socket = completeHandshake(address, "testUndo");
  //select a cell
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"} \n"));
  usleep(1* 1000000 /4);
  //make a change request
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"test\"} \n"));
  usleep(1* 1000000 /4);
  //change to something else
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"something\"} \n"));
  usleep(1* 1000000 /4);
  //undo 
  boost::asio::write(socket, boost::asio::buffer("{\"requestType\":\"undo\"} \n"));
  usleep(1* 1000000 /4);
  //check if it undid
  boost::system::error_code err;
  std::vector<char> tempRec = std::vector<char>(500);
  boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 500); 
  socket.read_some(receive, err);
  const char* data = boost::asio::buffer_cast<const char*>(receive);
  std::string temp(data);
  if(temp.find("test") != std::string::npos && !err)
  {
    std::cout << "pass \n" << std::endl;
  } else 
  {
    std::cout << "fail \n" << std::endl;
  }
}

void tester::testEditAndUndoDifferentClient(std::string address)
{
  boost::asio::ip::tcp::socket socket1 = completeHandshake(address, "testEditAndUndoDifferentClient");
  boost::asio::ip::tcp::socket socket2 = completeHandshake(address, "testEditAndUndoDifferentClient");
  boost::asio::write(socket1, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"} \n"));
  usleep(1* 1000000 /4);
  //make a change request
  boost::asio::write(socket1, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"test\"} \n"));
  usleep(1* 1000000 /4);
  //change to something else
  boost::asio::write(socket2, boost::asio::buffer("{\"requestType\":\"selectCell\",\"cellName\":\"A1\"} \n"));
  usleep(1* 1000000 /4);
  boost::asio::write(socket2, boost::asio::buffer("{\"requestType\":\"editCell\",\"cellName\":\"A1\", \"contents\":\"something\"} \n"));
  usleep(1* 1000000 /4);
  //undo 
  boost::asio::write(socket1, boost::asio::buffer("{\"requestType\":\"undo\"} \n"));
  usleep(1* 1000000 /4);
  //check if it undid
    boost::system::error_code err;
  std::vector<char> tempRec = std::vector<char>(500);
	boost::asio::mutable_buffers_1 receive = boost::asio::buffer(tempRec, 500); 
  socket2.read_some(receive, err);
  const char* data = boost::asio::buffer_cast<const char*>(receive);
  std::string temp(data);
  if(temp.find("test") != std::string::npos && !err)
  {
    std::cout << "pass \n" << std::endl;
  } else 
  {
    std::cout << "fail \n" << std::endl;
  }
}


void tester::testSpamEdits(std::string address)
{
  boost::asio::ip::tcp::socket socket = completeHandshake(address, "testSpamEdits");
  std::string selectFront = "{\"requestType\":\"selectCell\",\"cellName\":\"A";
  std::string selectEnd = "\"} \n";
  std::string editFront = "{\"requestType\":\"editCell\",\"cellName\":\"A";
  std::string editEnd = "\", \"contents\":\"test\"} \n";
  try
    {
      for(int i = 1; i < 100; i ++)
	{
	  std::string temp1 = selectFront + std::to_string(i) + selectEnd;
	  std::string temp2 = editFront + std::to_string(i) + editEnd;
	  boost::asio::write(socket, boost::asio::buffer(temp1));
	  usleep(1 * 1000000 / 4);
	  boost::asio::write(socket, boost::asio::buffer(temp2));
	  usleep(1 * 1000000 / 4);
	}
    } catch (std::exception& e)
    {
      std::cout << "fail \n" << std::endl;
    }

  std::cout << "pass \n" << std::endl;
}
