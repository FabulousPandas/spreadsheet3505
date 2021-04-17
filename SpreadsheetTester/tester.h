
#ifndef TESTER_H
#define TESTER_H


#include <iostream>

class tester
{

public:
static int testServerConnect();
static int testCircularDependency();
static int testSimultaniousEdit();
static int connectToClientTest();
static int basicServerMessage();
static int circularErrorTest();
static int simultaniousEditTest();

};

#endif
