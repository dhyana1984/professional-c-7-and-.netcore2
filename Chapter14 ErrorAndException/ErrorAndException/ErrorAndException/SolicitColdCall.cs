using System;
using System.IO;

namespace ErrorAndException
{
    public class SolicitColdCall
    {

        public static void ColdCallFileReaderLoop1(string filename)
        {
            var peopleToRing = new ColdCallFileReader();
            try
            {
                peopleToRing.Open(filename);
                for (int i = 0; i < peopleToRing.NPeopleToRing; i++)
                {
                    peopleToRing.ProcessNextPerson();
                }
                Console.WriteLine("All callers proceeded correctly");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"The file {filename} does not exist");
            }
            catch(CodeCallFileFormatException ex)
            {
                Console.WriteLine($"The file {filename} appears to have been corrupted");
                Console.WriteLine($"Details of problem are: {ex.Message}");
                if(ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception was: {ex.InnerException.Message}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Except occurred:\n{ex.Message}");
            }
            finally
            {
                peopleToRing.Dispose();
            }
        

        }

        internal class ColdCallFileReader : IDisposable
        {
            private FileStream _fs;
            private StreamReader _sr;
            private uint _nPeopleToRing;
            private bool _isDisposed = false;
            private bool _isOpen = false;

            public void Open(string fileName)
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException("peoplToRing");
                }
                _fs = new FileStream(fileName, FileMode.Open);
                _sr = new StreamReader(_fs);
                try
                {
                    string firstLine = _sr.ReadLine();
                    _nPeopleToRing = uint.Parse(firstLine);
                    _isOpen = true;
                }
                catch (FormatException ex)
                {
                    throw new CodeCallFileFormatException($"First line isn\'t and integer {ex}");
                }
            }

            public uint NPeopleToRing
            {
                get
                {
                    if (_isDisposed)
                    {
                        throw new ObjectDisposedException("peopleToRing");
                    }
                    if (!_isOpen)
                    {
                        throw new UnexceptedException("Attempted to access code-call that is not open");
                    }
                    return _nPeopleToRing;
                }
            }

            public void Dispose()
            {
                if (_isDisposed)
                {
                    return;
                }
                _isDisposed = true;
                _isOpen = false;
                _fs?.Dispose();
                _fs = null;
            }

            public void ProcessNextPerson()
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException("peopleToRing");
                }
                if (!_isOpen)
                {
                    throw new UnexceptedException("Attempted");
                }
                try
                {
                    string name = _sr.ReadLine();
                    if(name == null)
                    {
                        throw new CodeCallFileFormatException("Not enough names");
                    }
                    if (name[0] == 'B')
                    {
                        throw new SalesSpyFoundException(name);
                    }
                    Console.WriteLine(name);
                }
                catch(SalesSpyFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {

                }
            }
        }

    }
}


