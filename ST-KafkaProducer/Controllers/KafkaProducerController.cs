using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using ST_KafkaProducer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST_KafkaProducer.Controllers
{
    [Route("api/kafka")]
    [ApiController]
    public class KafkaProducerController : ControllerBase
    {
        private readonly ProducerConfig config = new ProducerConfig
        { BootstrapServers = "localhost:9092" };
        private readonly string topic = "simpletalk_topic";
        [HttpPost]
        public IActionResult Post([FromBody] Product Product)
        {
            return Created(string.Empty, SendToKafka(topic, Product));
        }
        private Object SendToKafka(string topic, Product Product)
        {
            using (var producer =
                 new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    return producer.ProduceAsync(topic, new Message<Null, string> { Value = Product.ToString() })
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }
            }
            return null;
        }
    }
}
