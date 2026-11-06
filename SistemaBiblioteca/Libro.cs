using System;

namespace SistemaBiblioteca
{
    public class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public int AnioPublicacion { get; set; }
        public int IdAutor { get; set; }
    }
}