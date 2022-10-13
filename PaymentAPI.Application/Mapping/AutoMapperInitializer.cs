using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Mapping
{
    /// <summary>
    /// Registra todos os mapas configurados na aplicação.
    /// </summary>
    public class AutoMapperInitializer
    {
        private Mapper _mapper;

        public AutoMapperInitializer()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps(System.Reflection.Assembly.GetExecutingAssembly())));
        }
        public Mapper GetMapper()
        {
            return _mapper;
        }
    }
}
