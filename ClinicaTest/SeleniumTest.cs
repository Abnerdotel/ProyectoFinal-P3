using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.Extensions;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ClinicaTest
{
    public class SeleniumTest : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5261/";
        private static ExtentReports _extentReports;
        private ExtentTest _test;

        public SeleniumTest()
        {
            var options = new EdgeOptions();
            _driver = new EdgeDriver(options);

            // Configurar ExtentReports
            if (_extentReports == null)
            {
                var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Tested", "ExtentReport.html");
                var sparkReporter = new ExtentSparkReporter(reportPath);
                sparkReporter.Config.DocumentTitle = "Reporte de pruebas";
                sparkReporter.Config.ReportName = "Pruebas con Selenium";
                sparkReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard;

                _extentReports = new ExtentReports();
                _extentReports.AttachReporter(sparkReporter);
            }
        }   
        private void Prueba(string nombrePrueba, string infoPrueba){
            _test = _extentReports.CreateTest(nombrePrueba).Info(infoPrueba);
        }

        //Pruebas de iniciar sesion en los 3 Roles
        [Fact]
        private void PruebaLoginAsAdmin()
        {
            Prueba("Prueba de inicio sesion como Admin", "Revisa que se inicie sesion correctamente como admin");
            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.FindElement(By.Id("DocumentoIdentidad")).SendKeys("40302010");
            _driver.FindElement(By.Id("Clave")).SendKeys("123");

            var button = _driver.FindElement(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button"));
            button.Click();

            Assert.Contains("/Home/Index", _driver.Url);
            _test.Pass("Se inicia sesion correctamente en el usuario Admin");
        }
        [Fact]
        private void PruebaLoginAsPacient()
        {
            Prueba("Prueba de inicio de sesion como Paciente", "Revisa que se inicie sesion correctamente como paciente");
            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.FindElement(By.Id("DocumentoIdentidad")).SendKeys("10203040");
            _driver.FindElement(By.Id("Clave")).SendKeys("123");

            var button = _driver.FindElement(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button"));
            button.Click();

            Assert.Contains("/Citas/Index", _driver.Url);
            _test.Pass("Se inicia sesion correctamente en el usuario paciente");

        }
        [Fact]
        private void PruebaLoginAsDoctor()
        {
            Prueba("Prueba de inicio de sesion como Doctor", "Revisa que se inicie sesion correctamente como Doctor");
            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.FindElement(By.Id("DocumentoIdentidad")).SendKeys("20000003");
            _driver.FindElement(By.Id("Clave")).SendKeys("20000003");

            var button = _driver.FindElement(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button"));
            button.Click();

            Assert.Contains("/Citas/CitasAsignadas", _driver.Url);
            _test.Pass("Se inicia sesion correctamente en el usuario Doctor");
        }
        [Fact]
        private void PruebaRegistroUsuario()
        {
            Prueba("Prueba de registro de datos en usuarios", "Registra un usuario en la base de datos");
            var waitUser = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_baseUrl);

            _driver.FindElement(By.Id("DocumentoIdentidad")).SendKeys("40302010");
            _driver.FindElement(By.Id("Clave")).SendKeys("123");

            var button = _driver.FindElement(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button"));
            button.Click();

            var UserButton = _driver.FindElement(By.XPath("/html/body/div[1]/div[1]/nav/div[1]/div/a[2]"));
            UserButton.Click();

            waitUser.Until(ExpectedConditions.ElementIsVisible(By.Id("btnNuevo")));  

            var NuevoButton = _driver.FindElement(By.Id("btnNuevo"));
            NuevoButton.Click();

            waitUser.Until(ExpectedConditions.ElementIsVisible(By.Id("mdData")));

            string usuarioNombre = "Prueba Usuario 1 Nombre";  
            _driver.FindElement(By.Id("txtNroDocumento")).SendKeys("20202020");
            _driver.FindElement(By.Id("txtNombres")).SendKeys(usuarioNombre);
            _driver.FindElement(By.Id("txtApellidos")).SendKeys("Prueba Usuario 1 Apellido");
            _driver.FindElement(By.Id("txtCorreo")).SendKeys("pruebausuario@usuario1.com");
            _driver.FindElement(By.Id("txtClave")).SendKeys("20202020");

            var GuardarUsuario = waitUser.Until(ExpectedConditions.ElementIsVisible(By.Id("btnGuardar")));
            Assert.True(GuardarUsuario.Enabled, "El botón Guardar no está habilitado.");
            GuardarUsuario.Click();

            waitUser.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.swal2-popup")));

            var btnOk = waitUser.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.swal2-confirm")));
            btnOk.Click();

            var searchBox = waitUser.Until(ExpectedConditions.ElementIsVisible(By.Id("dt-search-0")));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", searchBox);
            searchBox.SendKeys(usuarioNombre);

            var rows = waitUser.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#tbData tbody tr")));

            bool registroEncontrado = rows.Any(row =>
            {
                var columns = row.FindElements(By.CssSelector(".dt-type-numeric.sorting_1"));
                return columns.Any(col => col.Text.Contains("20202020"));
            });

            Assert.True(registroEncontrado, $"El registro con el nombre '{usuarioNombre}' no fue encontrado en los resultados de la búsqueda.");

            _test.Pass("Se registró correctamente el usuario");
        }
        [Fact]
        private void PruebaRegistroEspecialidad()
        {
            Prueba("Prueba de registro de datos en especialidad", "Registra una especialidad en la base de datos");
            _driver.Navigate().GoToUrl(_baseUrl);

            _driver.FindElement(By.Id("DocumentoIdentidad")).SendKeys("40302010");
            _driver.FindElement(By.Id("Clave")).SendKeys("123");

            var waitEspecialidad = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var loginButton = waitEspecialidad.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button")));
            loginButton.Click();

            var especialidadButton = waitEspecialidad.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[1]/nav/div[1]/div/a[3]")));
            especialidadButton.Click();

            var btnNuevo = waitEspecialidad.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnNuevo")));
            btnNuevo.Click();

            string especialidadNombre = "Prueba especialidad";
            waitEspecialidad.Until(ExpectedConditions.ElementIsVisible(By.Id("txtNombre"))).SendKeys(especialidadNombre);
            var btnGuardar = waitEspecialidad.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnGuardar")));
            btnGuardar.Click();

            waitEspecialidad.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.swal2-popup")));
            var btnOk = waitEspecialidad.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.swal2-confirm")));
            btnOk.Click();

            var searchBox = waitEspecialidad.Until(ExpectedConditions.ElementIsVisible(By.Id("dt-search-0")));
            searchBox.SendKeys(especialidadNombre); 

            var rows = waitEspecialidad.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#tbData tbody tr")));

            bool registroEncontrado = rows.Any(row =>
            {
                var columns = row.FindElements(By.ClassName("sorting_1"));
                return columns.Any(col => col.Text.Contains(especialidadNombre)); 
            });

            Assert.True(registroEncontrado, $"El registro con el nombre '{especialidadNombre}' no fue encontrado en los resultados de la búsqueda.");

            _test.Pass("Se registró correctamente la especialidad");
        }
        [Fact]
        private void PruebaRegistroDoctor()
        {
            Prueba("Prueba de registro de datos en doctores", "Registra un doctor en la base de datos");
            var waitDoctor = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_baseUrl);

            var documentoIdentidad = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("DocumentoIdentidad")));
            documentoIdentidad.SendKeys("40302010");

            var clave = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("Clave")));
            clave.SendKeys("123");

            var loginButton = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button")));
            loginButton.Click();

            var doctorButton = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[1]/nav/div[1]/div/a[4]")));
            doctorButton.Click();

            waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("btnNuevo")));

            var nuevoDoctorButton = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnNuevo")));
            nuevoDoctorButton.Click();

            waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("mdData")));

            string doctorNombre = "Prueba Doctor Nombre";

            var nroDocumento = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("txtNroDocumento")));
            nroDocumento.SendKeys("30303030");
            var nombres = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("txtNombres")));
            nombres.SendKeys(doctorNombre);
            var apellidos = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("txtApellidos")));
            apellidos.SendKeys("Prueba Doctor Apellido");

            var generoSelect = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("select2-cboGenero-container")));
            generoSelect.Click();
            var generoOption = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//li[text()='Masculino']")));
            generoOption.Click();

            var especialidadSelect = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("select2-cboEspecialidad-container")));
            especialidadSelect.Click();
            var especialidadOption = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//li[text()='Psicolog?a']")));
            especialidadOption.Click();

            var guardarDoctorButton = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("btnGuardar")));
            Assert.True(guardarDoctorButton.Enabled, "El botón Guardar no está habilitado.");
            guardarDoctorButton.Click();

            waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.swal2-popup")));

            // Espera y hace clic en el botón de confirmación
            var btnOk = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.swal2-confirm")));
            btnOk.Click();

            var searchBox = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("dt-search-0")));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", searchBox);
            searchBox.SendKeys(doctorNombre);

            var rows = waitDoctor.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#tbData tbody tr")));
            bool registroEncontrado = rows.Any(row =>
            {
                var columns = row.FindElements(By.CssSelector(".dt-type-numeric.sorting_1"));
                return columns.Any(col => col.Text.Contains("30303030"));
            });

            Assert.True(registroEncontrado, $"El registro con el número de documento '30303030' no fue encontrado en los resultados de la búsqueda.");
            _test.Pass("Se registró correctamente el doctor");
        }

        [Fact]
        private void PruebaRegistroHorarioDoctor()
        {
            Prueba("Prueba de registro de horario de doctor", "Registra un horario para un doctor en la base de datos");
            var waitDoctor = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl(_baseUrl);

            var documentoIdentidad = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("DocumentoIdentidad")));
            documentoIdentidad.SendKeys("40302010");

            var clave = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("Clave")));
            clave.SendKeys("123");

            var loginButton = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button")));
            loginButton.Click();

            var doctorButton = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[1]/nav/div[1]/div/a[5]")));
            doctorButton.Click();

            waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("btnNuevo")));

            var nuevoHorarioButton = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnNuevo")));
            nuevoHorarioButton.Click();

            waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("mdData")));

            string doctorNombre = "Carlos Martinez";

            var doctorSelect = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("select2-cboDoctor-container")));
            doctorSelect.Click();
            var doctorSpan = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//li[text()='20000000 - Carlos Martinez']")));
            doctorSpan.Click();

            var mesAtencionSelect = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("select2-cboMesAtencion-container")));
            mesAtencionSelect.Click();
            var mesAtencionSpan = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//li[text()='Diciembre']")));
            mesAtencionSpan.Click();

            var horaInicioAM = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("txtHoraInicioAM")));
            horaInicioAM.SendKeys("08:00");

            var horaFinAM = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("txtHoraFinAM")));
            horaFinAM.SendKeys("12:00");

            var horaInicioPM = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("txtHoraInicioPM")));
            horaInicioPM.SendKeys("14:00");

            var horaFinPM = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("txtHoraFinPM")));
            horaFinPM.SendKeys("18:00");

            var guardarHorarioButton = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnGuardar")));
            Assert.True(guardarHorarioButton.Enabled, "El botón Guardar no está habilitado.");
            guardarHorarioButton.Click();

            waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.swal2-popup")));

            var btnOk = waitDoctor.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button.swal2-confirm")));
            btnOk.Click();

            var searchBox = waitDoctor.Until(ExpectedConditions.ElementIsVisible(By.Id("dt-search-0")));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", searchBox);
            searchBox.SendKeys(doctorNombre);

            var rows = waitDoctor.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#tbData tbody tr")));
            bool registroEncontrado = rows.Any(row =>
            {
                var columns = row.FindElements(By.CssSelector("td")); 
                return columns[1].Text.Contains(doctorNombre); 
            });

            Assert.True(registroEncontrado, $"El registro del horario de doctor con el nombre '{doctorNombre}' no fue encontrado en los resultados de la búsqueda.");
            _test.Pass("Se registró correctamente el horario del doctor");
        }

        [Fact]
        private void PruebaAccesoAdmin()
        {
            Prueba("Prueba de acceso como Admin", "Revisa que se llege a todos los puntos de la web correctamente como admin");
            
            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.FindElement(By.Id("DocumentoIdentidad")).SendKeys("40302010");
            _driver.FindElement(By.Id("Clave")).SendKeys("123");

            var loginButton = _driver.FindElement(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button"));
            loginButton.Click();

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            var urls = new[]
            {
                new { Url = "http://localhost:5261/Home/Index#", Xpath = "/html/body/nav/a" },
                new { Url = "http://localhost:5261/Usuario/Index", Xpath = "/html/body/div[1]/div[1]/nav/div[1]/div/a[2]" },
                new { Url = "http://localhost:5261/Especialidad/Index", Xpath = "/html/body/div[1]/div[1]/nav/div[1]/div/a[3]" },
                new { Url = "http://localhost:5261/Doctor/Index", Xpath = "/html/body/div[1]/div[1]/nav/div[1]/div/a[4]" },
                new { Url = "http://localhost:5261/DoctorHorario/Index", Xpath = "/html/body/div[1]/div[1]/nav/div[1]/div/a[5]" }
            };

            foreach (var item in urls)
            {
                var linkButton = _driver.FindElement(By.XPath(item.Xpath));
                linkButton.Click();

                var currentUrl = _driver.Url;
                Assert.Equal(item.Url, currentUrl);

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            }

            _test.Pass("Se accede a todos los puntos de la web como admin");
        }

        [Fact]
        private void PruebaAccesoPaciente()
        {
            Prueba("Prueba de acceso como Paciente", "Revisa que se llege a todos los puntos de la web correctamente como Paciente");

            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.FindElement(By.Id("DocumentoIdentidad")).SendKeys("10203040");
            _driver.FindElement(By.Id("Clave")).SendKeys("123");

            var loginButton = _driver.FindElement(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button"));
            loginButton.Click();

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            var urls = new[]
            {
                new { Url = "http://localhost:5261/Citas/Index", Xpath = "/html/body/div[1]/div[1]/nav/div[1]/div/a[1]" },
                new { Url = "http://localhost:5261/HistorialCitas/Index", Xpath = "/html/body/div[1]/div[1]/nav/div[1]/div/a[2]" }
            };

            foreach (var item in urls)
            {
                var linkButton = _driver.FindElement(By.XPath(item.Xpath));
                linkButton.Click();

                var currentUrl = _driver.Url;
                Assert.Equal(item.Url, currentUrl);

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            }

            _test.Pass("Se accede a todos los puntos de la web como paciente");
        }

        [Fact]
        private void PruebaAccesoDoctor()
        {
            Prueba("Prueba de acceso como Doctor", "Revisa que se llege a todos los puntos de la web correctamente como Doctor");

            _driver.Navigate().GoToUrl(_baseUrl);
            _driver.FindElement(By.Id("DocumentoIdentidad")).SendKeys("20000000");
            _driver.FindElement(By.Id("Clave")).SendKeys("20000000");

            var loginButton = _driver.FindElement(By.XPath("/html/body/div[1]/div[1]/main/div/div/div/div/div[2]/form/div[3]/button"));
            loginButton.Click();

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            var url = "http://localhost:5261/Citas/CitasAsignadas";
            var xpath = "/html/body/div[1]/div[1]/nav/div[1]/div/a";

            var linkButton = _driver.FindElement(By.XPath(xpath));
            linkButton.Click();

            var currentUrl = _driver.Url;
            Assert.Equal(url, currentUrl);

            _test.Pass("Se accede a todos los puntos de la web como Doctor");
        }
        public void Dispose()
        {
            _driver.Quit();
            _extentReports.Flush();
        }
    }
}
