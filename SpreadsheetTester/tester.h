#ifndef TESTER_H
#define TESTER_H


#include <iostream>
#include <boost/asio.hpp>

class tester
{

public:
static void testServerConnect(std::string address);
static void testCircularDependency(std::string address);
static void testSimultaniousEdit(std::string address);
static void testIfServerSendsIntBackAsString(std::string address);
static void testIfServerSendsStringBackAsString(std::string address);
static void testClientDisconnectIsInfromed(std::string address);

private:
static boost::asio::ip::tcp::socket connectToServer(std::string serverip);
static boost::asio::ip::tcp::socket completeHandshake(std::string serverip, std::string username);
};

#endif