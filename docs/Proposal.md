# Object Space for Concurrent and Distributed Computing

by Tuan Tran, Daishiro Nishida

## Background

Linda, or Tuple Space (Gelernter, 1985) is a system supporting concurrent and distributed computing. The system revolves around a memory space storing tuples that clients could push and pop from. The three main command for clients of Linda are `write`, which sends a tuple to the Tuple Space, `read`, which reads a tuple matching the pattern, and `take`, which first `read` a tuple of the patterns and then delete that tuple. In these commands, items in a tuple could be replaced by its type for more flexibility.

Sample usage of the commands:

```python
    write("student", "Tuan Tran", 21, 3.6)
    read("student", "Daishiro Nishida", type(int), type(float))
```

The Linda API also specifies functions to read *all* tuples satisfy a pattern and specify blocking/non-blocking mode.

The drawbacks of this approach, however, is its lack of explicitness. For one, it is difficult to see the relation between items of the tuple. For example, for the tuple for student Tuan, what does `21` stands for? The student's age? Or maybe the number of courses taken by the student? Another problem is determining the type of the tuple. In our example the type is annotated by the first element of the tuple, however, it is naive to believe programmers will remember to do this constantly (if there is a way to break something, we will certainly do it).

We proposes an alternative for Linda by replacing the tuple space with an object space, passing a well-defined structure inside the space instead of a simple tuple. This will help us both the type and the field name problem, make it more familiar for a majority of programmers, as well as having a few interesting consequences discuss below.

## Proposal

### Basic usage:

```python
    class Student:
        def __init__(self, name, age, gpa):
            ...

    write(Student("Tuan Tran", 21, 3.6))
    read(Student)
```

### Subtyping

Using a object-oriented system, it is simple to have subtyping:

```python
    class CSStudent(Student):
        def __init__(self, name, age, gpa, unix):
            super().__init__(name, age)
            self.unix = unix

    write(CSStudent("Daishiro Nishida", 21, 3.85, "18nd6"))
    readAll(Student) # this reads both student Tuan and student Daishiro
```

### Filtering

Remember that in the original Linda we could call `read("student", "Daishiro Nishida", type(int), type(float))` which will give us only the student whose name is "Daishiro". For our new system we could replicate this behavior:

```python
    read(Student, lambda student: student.name == "Daishiro Nishida")
```

We could also do more complex checking that is not possible in the original schema:

```python
    read(Student, lambda student: student.age >= 20)
```

### Blocking timeout, number fetch limit

Other extensions to the system may include: a variable to specify the timeout limit, or the number of objects to fetch, etc

### Advantage of the new paradigm

As mention before, the new paradigm make it easy to specify a type on our input and output, which helps with clarity of the code and reduce errors.

The original system also does not have any limit on the number of parameters belong in the tuple, which makes it difficult to add many of the extension proposed above. Wrapping all information we want to push/pop to the shared memory space inside one parameter makes the API much more predictable and easy to use.

## Timeline

The deadline for the final project is December 15th. Due to the time limit we intend to take an existing open source implementation of Linda and modify it for our purpose. Currently we are investigating the implementation by the pSpaces project (https://github.com/pSpaces). The projected milestones are:

- Nov 26th: Build a simple example based on the original tuple space system (maybe a reimplementation of either project 2 or 3). Brainstorm the design and architecture for our extension.

- Dec 6th: Finish the MVP for the project: reading & writing objects, handle subtyping.

- Dec 15th: Polish our implementation. Implement examples for our system. Implement extensions if time allowed (advanced object filter, timeout, number of fetched objects, etc)
