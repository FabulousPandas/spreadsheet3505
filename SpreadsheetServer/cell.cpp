/*
 * Repesentation of a cell in a spreadsheet
 * Written by Malik Quader and Dylan Hansen
 */


#include "cell.h"
#include <regex>
	
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

		//if what's stored in the cell is a formula
		if(cell_contents[0] == '=')
		{
			//remove the = sign
			cell_contents = cell_contents.substr(1);

			// regex for valid cell name
			const std::regex r("[a-zA-Z_][a-zA-Z_0-9]*");
			std::smatch sm;

			//gets all of the valid cell names in the formula and puts it in the dependents vector
    			while (std::regex_search(cell_contents, sm, r)) 
			{
				dependents.push_back(sm[0]);
				cell_contents = sm.suffix();
    			}
		}

		return dependents;
	}

