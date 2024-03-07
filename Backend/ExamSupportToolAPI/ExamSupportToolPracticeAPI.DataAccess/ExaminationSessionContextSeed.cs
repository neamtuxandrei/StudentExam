using ExamSupportToolAPI.Domain;

namespace ExamSupportToolAPI.DataAccess
{
    public class ExaminationSessionContextSeed
    {
        public static async Task SeedAsync(ExaminationSessionDbContext context)
        {
            if (!context.SecretaryMembers.Any())
            {
                var secretaryList = new List<SecretaryMember>()
                {
                    SecretaryMember.Create("Vlad Secretar", "lupusvlad@gmail.com"),
                    SecretaryMember.Create("Andrei Secretar", "neamtuxandrei@gmail.com"),
                    SecretaryMember.Create("Alexandra Secretar", "pirvuletu.e.alexandra@gmail.com"),
                    SecretaryMember.Create("Catalin S", "catalin.sbora@gmail.com"),
                    SecretaryMember.Create("Mihnea Secretar", "m2869255@gmail.com"),

                };

                var studentList = new List<Student>()
                {
                    Student.Create("Alex Pirvuletu", "pirvuletu.elena.i7r@student.ucv.ro", "21DG67", 8, "XY", "Prof. Dl. Isaac"),
                    Student.Create("Andrei Neamtu", "neamtu.andrei.h7y@student.ucv.ro", "RDA21X", 5, "Aplicatie de aplicatie App", "Prof. Dl. Marian"),
                    Student.Create("Mihnea Sanda", "sanda.mihnea.t8k@student.ucv.ro","OPX15E",6,"Aplicatie ca idee","Conf. Dr. Ing. Vali Spaidar"),
                    Student.Create("Andrei student", "neamtuxandrei2@gmail.com", "GFN34F", 9, "New app", "Prof. Dl. Marian")
                };

                var committeeList = new List<CommitteeMember>() {
                    CommitteeMember.Create("Catalin S", "catalin.sbora@edu.ucv.ro"),
                    CommitteeMember.Create("Andrei C", "neamtuxandrei26@gmail.com")
                };

                var examinationSession = ExaminationSession.Create("Speciala", DateTime.Now, DateTime.Now.AddDays(3));

                examinationSession.SetSecretaryMemberId(secretaryList[2].Id);

                var examinationTickets = new List<ExaminationTicket>
                {
                    ExaminationTicket.Create(
                    1,
                    true,
                    "What is an algorithm?",
                    "Explain the concept of Big O notation.",
                    "What is object-oriented programming?",
                    "An algorithm is a step-by-step procedure for solving a problem.",
                    "Big O notation is used to describe the performance of an algorithm.",
                    "Object-oriented programming is a programming paradigm based on the concept of objects."
                    ),
                    ExaminationTicket.Create(
                    2,
                    true,
                    "What is the difference between stack and heap memory?",
                    "Explain the concept of recursion.",
                    "What is a database index?",
                    "Stack memory is used for function call management, while heap memory is used for dynamic memory allocation.",
                    "Recursion is a technique where a function calls itself to solve a problem.",
                    "A database index is a data structure that improves the speed of data retrieval operations in a database."
                    ),
                    ExaminationTicket.Create(
                    3,
                    true,
                    "What is the role of a compiler in programming?",
                    "Explain the concept of multi-threading.",
                    "What is SQL injection?",
                    "A compiler translates high-level code into machine code for execution.",
                    "Multi-threading allows multiple threads to run concurrently in a program.",
                    "SQL injection is a type of cyberattack where malicious SQL statements are inserted into input fields to manipulate a database."
                    ),
                    ExaminationTicket.Create(
                    4,
                    true,
                    "What is the significance of the Turing machine?",
                    "Explain the concept of artificial intelligence (AI).",
                    "What is a web API?",
                    "The Turing machine is a theoretical model of computation that laid the foundation for computer science.",
                    "Artificial intelligence (AI) is the simulation of human intelligence in machines.",
                    "A web API (Application Programming Interface) is a set of rules and protocols that allows different software applications to communicate with each other over the internet."
                    ),
                    ExaminationTicket.Create(
                    5,
                    true,
                    "What is the purpose of an operating system?",
                    "Explain the concept of binary search.",
                    "What is a linked list?",
                    "An operating system manages hardware resources and provides a user interface for software applications.",
                    "Binary search is an efficient search algorithm used to locate a specific item in a sorted list or array.",
                    "A linked list is a data structure in which elements are connected via pointers, allowing dynamic memory allocation."
                    ),
                    ExaminationTicket.Create(
                    6,
                    true,
                    "What is version control?",
                    "Explain the concept of a neural network.",
                    "What is the difference between TCP and UDP?",
                    "Version control is a system for tracking and managing changes to software code.",
                    "A neural network is a machine learning model inspired by the human brain's structure and function.",
                    "TCP (Transmission Control Protocol) is a reliable and connection-oriented protocol, while UDP (User Datagram Protocol) is a connectionless protocol used for fast data transmission."
                    ),
                    ExaminationTicket.Create(
                    7,
                    true,
                    "What is cloud computing?",
                    "Explain the concept of data normalization in databases.",
                    "What is the role of a front-end developer?",
                    "Cloud computing is the delivery of computing services, such as servers and storage, over the internet.",
                    "Data normalization is the process of organizing data in a database to reduce redundancy and improve data integrity.",
                    "A front-end developer is responsible for designing and implementing the user interface of a software application or website."
                    ),
                    ExaminationTicket.Create(
                    8,
                    true,
                    "What is a programming paradigm?",
                    "Explain the concept of SQL JOINs.",
                    "What is a cybersecurity vulnerability?",
                    "A programming paradigm is a style or approach to writing computer programs.",
                    "SQL JOINs are used to combine rows from multiple database tables based on a related column between them.",
                    "A cybersecurity vulnerability is a weakness or flaw in a system that can be exploited by attackers to compromise security."
                    ),
                    ExaminationTicket.Create(
                    9,
                    true,
                    "What is a software development life cycle (SDLC)?",
                    "Explain the concept of an API key.",
                    "What is a compiler optimization?",
                    "SDLC is a structured approach to planning, designing, and developing software systems.",
                    "An API key is a code that identifies and authorizes access to a specific API.",
                    "Compiler optimization is the process of improving the efficiency and performance of compiled code."
                    ),
                    ExaminationTicket.Create(
                    10,
                    true,
                    "What is the role of a database administrator (DBA)?",
                    "Explain the concept of responsive web design.",
                    "What is a denial-of-service (DoS) attack?",
                    "A database administrator (DBA) manages and maintains database systems.",
                    "Responsive web design is an approach to web design that ensures a web page's layout adapts to different screen sizes and devices.",
                    "A denial-of-service (DoS) attack is an attempt to disrupt the normal functioning of a computer system or network."
                    ),
                    ExaminationTicket.Create(
                    11,
                    true,
                    "What is the difference between a function and a method in programming?",
                    "Explain the concept of virtualization in computing.",
                    "What is the role of a data scientist?",
                    "In programming, a function is a standalone block of code, while a method is a function associated with a class or object.",
                    "Virtualization allows multiple virtual machines to run on a single physical server.",
                    "A data scientist analyzes and interprets complex data to inform business decisions."
                    ),
                    ExaminationTicket.Create(
                    12,
                    true,
                    "What is the purpose of a firewall?",
                    "Explain the concept of a cache memory.",
                    "What is the role of a system analyst?",
                    "A firewall is a network security device that filters incoming and outgoing network traffic.",
                    "Cache memory stores frequently accessed data for faster retrieval.",
                    "A system analyst assesses an organization's information systems and designs solutions to meet its needs."
                    ),
                    ExaminationTicket.Create(
                    13,
                    true,
                    "What is the difference between HTTP and HTTPS?",
                    "Explain the concept of a stack data structure.",
                    "What is ethical hacking?",
                    "HTTP is a protocol for transmitting data over the internet, while HTTPS adds a layer of security using encryption.",
                    "A stack is a linear data structure that follows the Last-In-First-Out (LIFO) principle.",
                    "Ethical hacking is a legal practice of identifying and addressing security vulnerabilities in computer systems."
                    ),
                    ExaminationTicket.Create(
                    14,
                    true,
                    "What is a compiler?",
                    "Explain the concept of a relational database management system (RDBMS).",
                    "What is a network protocol?",
                    "A compiler is a program that translates high-level code into machine code.",
                    "An RDBMS is a software system that manages relational databases.",
                    "A network protocol is a set of rules and conventions that govern how data is transmitted and received in a network."
                    ),
                    ExaminationTicket.Create(
                    15,
                    true,
                    "What is the role of an operating system kernel?",
                    "Explain the concept of cloud computing services.",
                    "What is the role of a software tester?",
                    "The operating system kernel manages hardware resources and provides core functionality.",
                    "Cloud computing services offer on-demand access to computing resources over the internet.",
                    "A software tester evaluates software applications to identify defects and ensure quality."
                    )
                };

                for (int i = 0; i < examinationTickets.Count; ++i)
                    examinationSession.AddExaminationTicket(examinationTickets[i]);
                examinationSession.AddStudent(studentList[0]);
                examinationSession.AddCommittees(committeeList);
                context.ExaminationSessions.Add(examinationSession);
                context.ExaminationTickets.AddRange(examinationTickets);
                context.SecretaryMembers.AddRange(secretaryList);
                context.Students.AddRange(studentList);

                await context.SaveChangesAsync();
            }
        }
    }
}

