import unittest
from selenium import webdriver
# import page

class ExampleSuite(unittest.TestCase):
    def setUp(self):
        self.driver = webdriver.Chrome()
        self.driver.get("https://www.calculator.net/")
    
    def test_example(self):
        self.assertTrue("Cal" in self.driver.title)
    def test_example_2(self):
        self.assertTrue("alc" in self.driver.title)
        self.subTest()
    
    def tearDown(self):
        self.driver.quit()
