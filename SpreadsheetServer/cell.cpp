#include "cell.h"
#include <regex>
#include <iostream> //TODO REMOVE
	
	/*
	* Cell constructor
	*/
	cell::cell(std::string name)
	{
		cell_name = name;
		undo_stack.push("");
	}
	/*
	* changes cell contenst to param edit
	*/
	void cell::add_edit(std::string edit)
	{
		//add edit to stack
		undo_stack.push(edit);
		
	}
	/*
	* undoes the act of reverting
	* if the stack was empty then "empty"
	* is returned
	*/
	std::string cell::undo_revert()
	{
		if (redo_stack.empty())
			return "empty";

		std::string popped;
		//save popped edit
		popped = redo_stack.top();
		redo_stack.pop();
		//push popped edit onto redo stack
		undo_stack.push(popped);

		return popped;
	}

	/*
	* removes last edit from the stack
	*/
	void cell::remove_edit()
	{
		undo_stack.pop();
	}
	/*
	* reverts cell to last change
	* or if the stack was empty then "empty"
	* is returned
	*/
	std::string cell::revert()
	{
		if (undo_stack.empty())
			return "empty";

		std::string popped;
		//save popped edit
		popped = undo_stack.top();
		undo_stack.pop();
		//push popped edit onto redo stack
		redo_stack.push(popped);

		return popped;
	}

	/*
	* returns the contents of the cell
	*/
	std::string cell::cell_content()
	{
		return undo_stack.top();
	}

	/*
	 * returns a list of the cells that this cell uses
	 */
	std::vector<std::string> cell::get_dependent_list()
	{
		std::string cell_contents = cell_content();
		std::vector<std::string> dependents;

		std::cout << "IN GET DEPENDENT LIST" << std::endl;

		if(cell_contents[0] == '=')
		{
			cell_contents = cell_contents.substr(1);
			std::cout << "REAL IN GET DEPENDENT LIST: " << cell_contents << std::endl;

			const std::regex r("[a-zA-Z_][a-zA-Z_0-9]*");
			std::smatch sm;

    			while (std::regex_search(cell_contents, sm, r)) 
			{
				std::cout << sm[0] << std::endl;
				dependents.push_back(sm[0]);
				cell_contents = sm.suffix();
    			}
		}

		return dependents;
	}

