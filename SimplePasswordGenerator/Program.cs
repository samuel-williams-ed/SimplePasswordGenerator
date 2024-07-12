// See https://aka.ms/new-console-template for more information
using PasswordGenerator;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace passwordApp
{

    class Program
    {

        class Option
        {
            public string? num;
            public string? text;
        }

        // create Single Thread Apartment function, 
        // required for System.Windows.Forms (copying to Clipboard)
        [STAThread]
        static void Main(string[] args)
        {

            // Ensure the application runs in STA mode (required for clipboard operations)
            // Though text and UI is not relevant for cmdln app - these settings ensure compatibility 
            // and rendering consistency for applicationas relying on this app 
            // (ie., the clipboard) when using some modern and/or international fonts
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // One source of truth for password generation options
            Option[] options = new Option[] {
                new() {num = "0", text="default, 16 letter"},
                new() {num = "1", text="short"}, 
                new() {num = "2", text="4-number PIN"},
                new() {num = "3", text="6-number PIN"},
                new() {num = "4", text="alphanumeric"},
                new() {num = "5", text="long"},
                new() {num = "6", text="long-alphanumeric"}
            };


            // variables (with standard values, to be overwritten)
            var userSelectedText = "Standard 16 alphanumeric letters, upper and lower case, with symbols.";
            var pwd = new Password();

            Console.WriteLine("\nWelcome to password generator:");
            string? userInput = "0";

            while ( userInput != null && !userInput.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                generatePassword();
                Console.WriteLine("\nHit enter to generate another password. \n Press 'q' to quit");
                userInput = Console.ReadLine();
            };
            
            void generatePassword()
            {

                // get user input
                Console.WriteLine("Here, what kind of password would you like?");
                foreach (Option opt in options) 
                { 
                    Console.WriteLine($"{opt.num}: {opt.text}"); 
                }
                Console.WriteLine("---");
                var userSelection = Console.ReadLine();


                // logic to handle user selection
                pwd = new Password();
                userSelectedText = "Standard 16 alphanumeric letters, upper and lower case, with symbols.";
                if (userSelection == options[1].num)
                {
                    pwd = new Password(6);
                    userSelectedText = options[1].text;
                }
                if (userSelection == options[2].num)
                {
                    pwd = (Password) new Password(4).IncludeNumeric();
                    userSelectedText = options[2].text;
                }
                if (userSelection == options[3].num)
                {
                    pwd = (Password) new Password(6).IncludeNumeric();
                    userSelectedText = options[3].text;
                }
                if (userSelection == options[4].num)
                {
                    pwd = (Password) new Password().IncludeNumeric().IncludeLowercase().IncludeUppercase();
                    userSelectedText = options[4].text;
                }
                if (userSelection == options[5].num)
                {
                    pwd = (Password) new Password(32);
                    userSelectedText = options[5].text;
                }
                if (userSelection == options[6].num)
                {
                    pwd = (Password) new Password(32).IncludeNumeric().IncludeLowercase().IncludeUppercase();
                    userSelectedText = options[6].text;
                }

                // By default, all characters available for use and a length of 16
                // Will return a random password with the default settings 
                var password = pwd.Next();
                Console.WriteLine($"Generating password: {userSelectedText}");

                Clipboard.SetText(password);
                Console.WriteLine(password.ToString());
                Console.WriteLine($"(This has been copied to your clipboard!)");
            }
        }
    }
}


