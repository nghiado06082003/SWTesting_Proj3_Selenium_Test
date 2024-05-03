use std::{fs, path::Path};

use async_stream::stream;
use futures::Stream;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub enum Grade {
    #[serde(rename = "-")]
    None,
    #[serde(rename = "A")]
    A,
    #[serde(rename = "A-")]
    AM,
    #[serde(rename = "A+")]
    AP,
    #[serde(rename = "B")]
    B,
    #[serde(rename = "B-")]
    BM,
    #[serde(rename = "B+")]
    BP,
    #[serde(rename = "C")]
    C,
    #[serde(rename = "C-")]
    CM,
    #[serde(rename = "C+")]
    CP,
    #[serde(rename = "D")]
    D,
    #[serde(rename = "D-")]
    DM,
    #[serde(rename = "D+")]
    DP,
    #[serde(rename = "F")]
    F,
    #[serde(rename = "P")]
    P,
    #[serde(rename = "NP")]
    NP,
}

#[derive(Serialize, Deserialize)]
pub struct Input {
    pub course_name: String,
    pub credits: isize,
    pub grade: Grade,
}

#[derive(Serialize, Deserialize)]
pub enum Output {
    Error(String),
    Ok(usize),
}

pub struct TestCase {
    pub input: Input,
    pub output: Output,
}

pub fn read_test_suites(test_root_dir: String) -> impl Stream<Item = TestCase> { 
    let test_case_folders = fs::read_dir(&test_root_dir).expect(format!("Folder {test_root_dir} should exist").as_str());
    
    stream! {
        for test_case_folder in test_case_folders {
            let abs_path = Path::new(test_root_dir.as_str()).join(test_case_folder.unwrap().file_name());
            let input_file_path = abs_path.join("input.txt");
            let input_file_path = input_file_path.to_str().unwrap();
            let output_file_path = abs_path.join("output.txt");
            let output_file_path = output_file_path.to_str().unwrap();
        
            let input = serde_json::from_str(&fs::read_to_string(input_file_path).expect(format!("Should have been able to read {input_file_path}").as_str())).expect(format!("File {input_file_path} doesn't have the correct format").as_str());
            let output = serde_json::from_str(&fs::read_to_string(output_file_path).expect(format!("Should have been able to read {}", output_file_path).as_str())).expect(format!("File {output_file_path} doesn't have the correct format").as_str());
        
            yield TestCase { input, output }
        }
    }
}

