
#ifndef TESTER_H
#define TESTER_H


#include <iostream>
#include <boost/asio.hpp>

class tester
{

public:
static int testServerConnect();
static int testCircularDependency();
static int testSimultaniousEdit();

private:
static boost::asio::ip::tcp::socket connectToServer(std::string serverip);
  
};

#endif
