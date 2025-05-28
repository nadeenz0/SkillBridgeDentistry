using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkillBridgeDentistry1.Dtos;


namespace CoreLayer.Services
{
    public interface IAIService
    {
        Task<AIResponseDto> PredictDiseaseAsync(string imagePath);
    }
}
