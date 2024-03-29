﻿// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

/// Author: Khris Thammavong


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<string, HashSet<string>> dependents;
        private Dictionary<string, HashSet<string>> dependees;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
            Size = 0;
        }

        private int p_size;
        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return p_size; }
            private set { p_size = value; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get 
            {
                if (!dependees.ContainsKey(s))
                    return 0;
                return dependees[s].Count; 
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (!dependents.ContainsKey(s))
                return false;
            return dependents[s].Count != 0;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (!dependees.ContainsKey(s))
                return false;
            return dependees[s].Count != 0;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (!dependents.ContainsKey(s))
                return new HashSet<string>();
            return new HashSet<string>(dependents[s]);
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (!dependees.ContainsKey(s))
                return new HashSet<string>();
            return new HashSet<string>(dependees[s]);
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (s == null || t == null) //arguments can't be null
                throw new ArgumentNullException();

            //Handles Dependents
            if (dependents.ContainsKey(s)) //if s has already been used as a dependee previously
            {
                if (dependents[s].Contains(t)) //if the ordered pair has already been put in the dependency graph, there's no need to add it
                    return;

                dependents[s].Add(t);
            }
            else //if not, creates a new set for the key s, adds t to it, and creates the new key-value pair
            {
                HashSet<string> set = new HashSet<string>();
                set.Add(t);
                dependents[s] = set;
            }

            //Handles Dependees
            if (dependees.ContainsKey(t)) //if t has already been used as a dependent previously
            {
                dependees[t].Add(s);
            }
            else //if not, creates a new set for the key t, adds s to it, and creates the new key-value pair
            {
                HashSet<string> set = new HashSet<string>();
                set.Add(s);
                dependees[t] = set;
            }

            Size++;
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null) //arguments can't be null
                throw new ArgumentNullException();

            if (dependents.ContainsKey(s))
            {
                if (dependents[s].Contains(t))
                {
                    dependents[s].Remove(t);
                    dependees[t].Remove(s);
                    Size--;
                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null) //arguments can't be null
                throw new ArgumentNullException();

            foreach (string r in GetDependents(s))
                RemoveDependency(s, r);
            foreach (string t in newDependents)
                AddDependency(s, t);
            

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (s == null || newDependees == null) //arguments can't be null
                throw new ArgumentNullException();

            foreach (string r in GetDependees(s))
                RemoveDependency(r, s);
            foreach (string t in newDependees)
                AddDependency(t, s);
            
        }

    }

}
