#ifndef TESTER_H
#define TESTER_H


#include <iostream>
#include <boost/asio.hpp>

class tester
{

public:
static int testServerConnect(std::string address);
static int testCircularDependency(std::string address);
static int testSimultaniousEdit(std::string address);

private:
static boost::asio::ip::tcp::socket connectToServer(std::string serverip);
static boost::asio::ip::tcp::socket tester::completeHandshake(std::string serverip, std::string username)
};

#endif