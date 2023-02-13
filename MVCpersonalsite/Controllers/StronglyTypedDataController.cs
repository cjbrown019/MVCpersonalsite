using MVCpersonalsite.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit; //Added for access to MimeMessage class
using MailKit.Net.Smtp; //Access to the SmtpClient class

namespace MVCpersonalsite.Controllers
{
    public class StronglyTypedDataController : Controller
    {
        //We won't be using an index action or view,
        //so we can simply comment out/delete the one is provided.

        #region Adding Credentials to appsettings.json

        #endregion

        #region config
        private readonly IConfiguration _config;

        public StronglyTypedDataController(IConfiguration config)
        {
            _config = config;
        }

        #endregion

        #region Code genneration
        //Contro11er Actions are meant to handle certain types of requests. The most common request
        // is GET, which is used to request info to load a page. We will also create actions
        // to handle POST requests, which are used to send info to the app.
        [HttpGet]

        public IActionResult Contact()
        {
            return View();
            //We want the info from our Contact form to use the ContactViewMode1 we created.
            //We can generate a View using this information automatically.

            #region Code Generation Steps

            //1. Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution
            //2. Go to the Browse tab and search for Microsoft.VisualStudio.Web
            //3. Click Microsoft.VisualStudio.Web.CodeGeneration.Design (6.0.x)
            //4. On the right, check the box next to the CORE1 project, then click "Install"
            //5. Once installed, return here and right click the Contact action
            //6. Select Add View, then select the Razor View template and click "Add"
            //7. Enter the following settings:
            //      - View Name: Contact
            //      - Template: Create
            //      - Model Class: ContactViewModel
            //8. Leave all other settings as-is and click "Add"

            #endregion

        }
        #endregion

        //We need to handle what to do when the user submits the contact form.For this,
        //we will make another Contact action (overloaded), this time to handle the POST request.
        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            //When a class has validation attributes, that validation should be checked
            // BEFORE attempting to process any of the data they provided.
            if (!ModelState.IsValid)
            {
                // Send them back to the form.We can pass the object to the View
                // so the form will contain the original information they provided.
                return View(cvm);
            }
            // To handle send in the email we'll need to install a NuGet package.

            //MIME -> Multipurpose Internet Mail Extensions - Allows email to contain
            //Information other than ASCII -> Audio, Video, Images, and HTML.

            //SMTP - Simple Mail Transfer Protocol -> unencrypted - an internet protocol
            //(like http) that specializes in the collection and transfer of email data.

            #region Email Setup Steps & Email Info

            //1. Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution
            //2. Go to the Browse tab and search for NETCore.MailKit
            //3. Click NETCore.MailKit
            //4. On the right, check the box next to the CORE1 project, then click "Install"
            //5. Once installed, return here
            //6. Add the following using statements & comments:
            //      - using MimeKit; //Added for access to MimeMessage class
            //      - using MailKit.Net.Smtp; //Added for access to SmtpClient class
            //7. Once added, return here to continue coding email functionality
            #endregion

            //Create and format the message content we will recieve from the contact form.
            string message = $"You have recieved a new email from your site's contact form!<br>" +
                $"Sender: {cvm.Name}<br>" +
                $"Company: {cvm.Company}<br>" +
                $"Email: {cvm.Email}<br>" +
                $"Message: {cvm.Message}";
            //Build Mime message object to assist with transporting the email information.
            var mm = new MimeMessage();

            //We can access the credentials for this email user from the appsettings.json file
            mm.From.Add(new MailboxAddress("Personal Site", _config.GetValue<string>("Credentials:Email:User")));

            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            mm.Subject = cvm.Subject;
            //The Body of the message will be formatted with the string we created above.
            mm.Body = new TextPart("HTML") { Text = message };

            //We can set the priority of a message to "Urgent" so it will be flagged in our email client.
            mm.Priority = MessagePriority.Urgent;

            //Using directive - create the SmtpClient object used to actually send the email.
            //Once all of the code is done, it will close any open connections and dispose of the object
            //for us
            using (var client = new SmtpClient())
            {
                try
                {
                    //Connect to the mail server using credentials in our appsettings.json
                    //client.Connect(_config.GetValue<string>("Credentials:Email:Client"));
                    //handle alternate ports
                    client.Connect(_config.GetValue<string>("Credentials:Email:Client"), 8889);
                    //Login to the mail sever using your credentials
                    client.Authenticate(
                        //UserName
                        _config.GetValue<string>("Credentials:Email:User"),
                        //Password
                        _config.GetValue<string>("Credentials:Email:Password")
                        );
                    //send the email
                    client.Send(mm);
                }
                //Handle any issues
                catch (Exception ex)
                {
                    //if there are ANY issues, we can stor an error message in a viewBag variable
                    //and display in the View.
                    ViewBag.ErrorMessage = $"There was an error processing your request. Please" +
                        $" try again later. <br>" +
                        $"Error Message: {ex.Message} ";
                    //return the user to the View with their form information intact.
                    return View(cvm);
                }//end try catch
            }//end using SmtpClient
            //If all goes well, return a View that displays a confirmation message to the user
            //that their email was sent.
            return View("EmailConfirmation", cvm);
        }
    }
}
