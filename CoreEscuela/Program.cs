﻿using System;
using System.Collections.Generic;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            //Eventos
            //AppDomain -> Es donde se ejecuta cada una de las aplicaciones
            //CurrenteDomain -> Es donde esta ejecunatndo este programa
            AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;

            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");
            //Printer.Beep(10000, cantidad: 10);
            //ImpimirCursosEscuela(engine.Escuela);
            Dictionary<int, string> dicccionario = new Dictionary<int, string>();

            dicccionario.Add(10, "JuanK");

            dicccionario.Add(23, "Lorem Ipsum");

            foreach (var keyValPair in dicccionario)
            {
                WriteLine($"Key: {keyValPair.Key} Valor: {keyValPair.Value}");
            }

            var dictmp = engine.GetDiccionarioObjetos();
            engine.ImprimirDiccionario(dictmp,true);
        }

        private static void AccionDelEvento(object sender, EventArgs e)
        {
            Printer.WriteTitle("SALIENDO");
            Printer.Beep(3000, 1000, 3);
            Printer.WriteTitle("SALIO");
        }

        private static void ImpimirCursosEscuela(Escuela escuela)
        {

            Printer.WriteTitle("Cursos de la Escuela");

            //Solo verificacmos cursos en caso de escuela se igual a NULL esto lo ahce el operqador '?'
            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre}, Id  {curso.UniqueId}");
                }
            }
        }
    }
}