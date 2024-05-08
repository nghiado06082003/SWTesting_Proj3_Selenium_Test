import unittest
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import json

class TimeCalculatorSuite(unittest.TestCase):
    def setUp(self):
        self.driver = webdriver.Chrome()
        self.base_url = "https://www.calculator.net/time-calculator.html"
        with open("./test_cases/testcase_Time_Calculator.json") as fp:
            self.testcases = json.load(fp)
    
    def test_boundary_value_analysis(self):
        passed = 0
        for testcase in self.testcases:
            with self.subTest(testcase["test_name"], testcase=testcase):
                driver = self.driver
                input = testcase["input"]
                expected_output = testcase["expected_output"]
                
                driver.get(self.base_url)
                driver.set_window_size(1024, 812)
                driver.find_element(By.ID, "tcday1").send_keys(input["tcday1"])
                driver.find_element(By.ID, "tchour1").send_keys(input["tchour1"])
                driver.find_element(By.ID, "tcminute1").send_keys(input["tcminute1"])
                driver.find_element(By.ID, "tcsecond1").send_keys(input["tcsecond1"])
                driver.find_element(By.ID, "tcday2").send_keys(input["tcday2"])
                driver.find_element(By.ID, "tchour2").send_keys(input["tchour2"])
                driver.find_element(By.ID, "tcminute2").send_keys(input["tcminute2"])
                driver.find_element(By.ID, "tcsecond2").send_keys(input["tcsecond2"])
                driver.find_element(By.CSS_SELECTOR, "tr:nth-child(3) .cbcontainer:nth-child(2) > .rbmark").click()
                driver.find_element(By.NAME, "x").click()
                WebDriverWait(self.driver, 0.5).until(
                    EC.visibility_of_element_located((By.CSS_SELECTOR, ".h2result"))
                )
                
                failed_msg = "FAIL TESTCASE " + testcase["test_name"]
                result_row = driver.find_element(By.CSS_SELECTOR, "table:nth-child(5) tr:nth-child(4)")
                self.assertEqual(result_row.find_element(By.CSS_SELECTOR, "td:nth-child(2)").text, expected_output["days"], failed_msg)
                self.assertEqual(result_row.find_element(By.CSS_SELECTOR, "td:nth-child(3)").text, expected_output["hours"], failed_msg)
                self.assertEqual(result_row.find_element(By.CSS_SELECTOR, "td:nth-child(4)").text, expected_output["minutes"], failed_msg)
                self.assertEqual(result_row.find_element(By.CSS_SELECTOR, "td:nth-child(5)").text, expected_output["seconds"], failed_msg)
                passed += 1
                print("PASS TESTCASE " + testcase["test_name"])
        print(f"PASSED {passed} OF {len(self.testcases)} TESTCASES")
    
    def tearDown(self):
        self.driver.quit()
