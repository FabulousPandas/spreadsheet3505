#include "cell.h"

	cell(std::string name)
	{
		cell_name = name;

	}
	/*
	* changes cell contenst to param edit
	*/
	void add_edit(std::string edit)
	{
		//add edit to stack
		undo_stack.push_back(edit);
		
		//clear redo stack
		for (int i = 0; i < redo_stack.size(); i++)
			redo_stack.pop();
	}
	/*
	* changes cell to most recent undo
	*/
	std::string redo_cell()
	{
		std::string popped;
		//save popped edit
		popped = redo_stack.pop();
		//push popped edit onto redo stack
		undo_stack.push_back(popped);

		return popped;
	}
	/*
	* changes cell to most recent edit
	*/
	std::string undo_cell()
	{
		std::string popped;
		//save popped edit
		popped = undo_stack.pop();
		//push popped edit onto redo stack
		redo_stack.push_back(popped);

		return popped;
	}

	/*
	*
	*/
	std::string cell_contents()
	{
		return undo_stack.top();
	}