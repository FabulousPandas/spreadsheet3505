#include <iostream>
#include "tester.h"



int main()
{
  //call all tests here
  tester::testServerConnect();
  return 0; //number of tests failed
}


int tester::testServerConnect()
{
  //connect with socket to sever and start handshake

  return 0; //return 1 if error
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


/* Client tests */

int connectToClientTest()
{
  //attempt to connect to the client via socket

  return 0;
}

int tester::basicServerMessage()
{
  //send a basic edit message from the server

  return 0;
}

int tester::circularErrorTest()
{
  //send a error message for a circular dependency to the client
  
  return 0;
}

int tester::simultaniousEditTest()
{
  //send an error for a simultanious edit to the client
  
  return 0;
}

