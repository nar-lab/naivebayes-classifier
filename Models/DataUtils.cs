using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NBayes.Models
{
    public class DataUtils
    {
        private const string TARGETNAME = "Acceptence";

        /// <summary>
        /// Initial load method for training data.
        /// </summary>
        /// <returns></returns>
        public static List<Person> SeedTrainingData()
        {
            return new List<Person>()
                             {
                                new Person() {Education = "Middle",Age="Old",Sex="Male",Acceptence = "Yes"},
                                new Person() {Education = "Low",Age="Teen",Sex="Male",Acceptence = "No"},
                                new Person() {Education = "High",Age="Middle",Sex="Female",Acceptence = "No"},
                                new Person() {Education = "Middle",Age="Middle",Sex="Male",Acceptence = "Yes"},
                                new Person() {Education = "Low",Age="Middle",Sex="Male",Acceptence = "Yes"},
                                new Person() {Education = "High",Age="Old",Sex="Female",Acceptence = "Yes"},
                                new Person() {Education = "Low",Age="Teen",Sex="Female",Acceptence = "No"},
                                new Person() {Education = "Middle",Age="Middle",Sex="Female",Acceptence = "Yes"}
                             };
        }

        /// <summary>
        /// this method calculates the probability for whole input variables
        /// by target variables
        /// this method should be changed for additional cases like:
        /// for numerical input variables, calculating the normal distribution (PDF)
        /// for multinomial target variable, changing tolerance value and Yes/No decoding.
        /// </summary>
        /// <param name="featureMap"></param>
        /// <returns></returns>
        public static List<FeatureProb> GetFeatureProb(List<Person> featureMap)
        {
            //Get all features and feature types from training data
            PropertyInfo[] piFeature = featureMap[0].GetType().GetProperties();

            var targetFeatureProp = piFeature.FirstOrDefault(x => x.Name == TARGETNAME);
            //This variable will be filled by feature. It could be Education, Age, Sex in this example.
            string featureName;
            //This variable will be used to calculate probabilities as a summary, feature name, feature level,
            //feature level prob and if feature level's value equals to zero, we will adjust the probability
            // with a low tolerance
            var featureProbs = new List<FeatureProb>();
            //navigating in the features
            foreach (var prop in piFeature)
            {
                //initiliazing a temp list for a feature in training set.
                //this list can keep character set
                //TODO: we will rearrange this list that it could keep dynamic type for numeric values.
                var featureVals = new List<FeatureRelated>();
                //getting the property name, Education, Age, Sex, Acceptence
                featureName = prop.Name;
                //checking the property type
                //TODO: this condition compare just character data, we will compare numeric data later.
                if (prop.PropertyType.Name != "String")
                    continue;
                if (featureName == TARGETNAME)
                    continue;
                //navigating in training set and copy feature data to the temp list
                foreach (var feature in featureMap)
                {
                    if (targetFeatureProp != null)
                        featureVals.Add(
                            new FeatureRelated()
                            {
                                Feature = prop.GetValue(feature, null).ToString(),
                                Target = targetFeatureProp.GetValue(feature, null).ToString()
                            });
                }

                //grouping temp list and calculating the frequencies for each level
                var featFreq = featureVals.GroupBy(x => x.Feature)
                    .Select(g => new {
                        Text = g.Key,
                        CountY = g.Count(a => a.Target == TargetLevels.Yes.ToString()),
                        CountN = g.Count(a => a.Target == TargetLevels.No.ToString())
                    });
                //total count for each feature
                var featureYCnt = featureVals.Count(x => x.Target == TargetLevels.Yes.ToString());
                var featureNCnt = featureVals.Count(x => x.Target == TargetLevels.No.ToString());
                //if any frequency of a level equals to zero, calculate the adjusted probability
                var containsZero = featFreq.Count(x => Math.Abs(x.CountY) < 0.001 || Math.Abs(x.CountN) < 0.001) == 0 ? true : false;
                //navigating in the grouped data
                foreach (var freq in featFreq)
                {
                    //calculating probabilities by Yes / No Acceptence counts.
                    double featureYProb = (double)freq.CountY / featureYCnt;
                    double featureNProb = (double)freq.CountN / featureNCnt;
                    //initializing adjusted probs
                    double adjFeatureYProb = featureYProb;
                    double adjFeatureNProb = featureNProb;
                    //if any cell contains zero, calculate the adjusted probs with a low tolerance
                    //TODO: due to target variable has binomial values, tolerance value is selected around 1/2
                    //TODO: to be changed the static tolerance value depends on target binomial or multinomial
                    if (containsZero)
                    {
                        adjFeatureYProb = (freq.CountY + 0.5) / (featureYCnt + 1);
                        adjFeatureNProb = (freq.CountN + 0.5) / (featureNCnt + 1);
                    }
                    //adding calculated vars to final proability matrix list 
                    featureProbs.Add(new FeatureProb()
                    {
                        VarName = featureName,
                        LevelName = freq.Text,
                        LevelYCnt = freq.CountY,
                        LevelYProb = featureYProb,
                        LevelYAdjProb = adjFeatureYProb,
                        LevelNCnt = freq.CountN,
                        LevelNProb = featureNProb,
                        LevelNAdjProb = adjFeatureNProb
                    });
                }
            }

            return featureProbs;
        }
    }
}